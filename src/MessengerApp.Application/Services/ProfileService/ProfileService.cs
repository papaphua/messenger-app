using System.Web;
using AutoMapper;
using MessengerApp.Application.Dtos.Profile;
using MessengerApp.Application.Services.UserService;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Services.ProfileService;

public sealed class ProfileService : IProfileService
{
    private readonly IMapper _mapper;
    private readonly IEmailSender _sender;
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;

    public ProfileService(UserManager<User> userManager, IMapper mapper, IEmailSender sender, IUserService userService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _sender = sender;
        _userService = userService;
    }

    public async Task<Result<ProfileDto>> GetProfileAsync(string? userId)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result<ProfileDto>
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        return new Result<ProfileDto>
        {
            Data = _mapper.Map<User, ProfileDto>(user)
        };
    }

    public async Task<Result> UploadProfilePictureAsync(string? userId, ProfilePictureDto profilePictureDto)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        user.ProfilePicture = profilePictureDto.ProfilePicture;
        await _userManager.UpdateAsync(user);

        return new Result
        {
            Message = Results.ProfilePictureUpdated
        };
    }

    public async Task<Result> UpdateUserInfoAsync(string? userId, ProfileInfoDto infoDto)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        _mapper.Map(infoDto, user);
        var updateResult = await _userManager.UpdateAsync(user);

        return new Result
        {
            Succeeded = updateResult.Succeeded,
            Message = Result.IdentityResultsToMessage(updateResult) ?? Results.UserProfileUpdated
        };
    }

    public async Task<Result> ChangePasswordAsync(string? userId, ChangePasswordDto passwordDto)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        if (user.IsExternal)
            return new Result
            {
                Succeeded = false,
                Message = Results.ExternalUser
            };

        var passwordResult =
            await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

        return new Result
        {
            Succeeded = passwordResult.Succeeded,
            Message = Result.IdentityResultsToMessage(passwordResult) ?? Results.UserPasswordChanged
        };
    }

    public async Task<Result> RequestEmailConfirmationAsync(string? userId)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        if (user.IsExternal)
            return new Result
            {
                Succeeded = false,
                Message = Results.ExternalUser
            };

        if (user.EmailConfirmed)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailAlreadyConfirmed
            };

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = AddTokenToUrl(Links.EmailConfirmationLink, token);

        await _sender.SendEmailAsync(user.Email!, Emails.EmailConfirmationSubject,
            Emails.EmailConfirmationMessage(link));

        return new Result
        {
            Message = Results.EmailConfirmationRequested
        };
    }

    public async Task<Result> ConfirmEmailAsync(string? userId, string token)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        if (user.EmailConfirmed)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailAlreadyConfirmed
            };

        var confirmationResult = await _userManager.ConfirmEmailAsync(user, token);

        return new Result
        {
            Succeeded = confirmationResult.Succeeded,
            Message = Result.IdentityResultsToMessage(confirmationResult) ?? Results.UserEmailConfirmed
        };
    }

    public async Task<Result> RequestEmailChangeAsync(string? userId, ProfileEmailDto emailDto)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        if (user.IsExternal)
            return new Result
            {
                Succeeded = false,
                Message = Results.ExternalUser
            };

        var userWithThisEmail =
            await _userManager.Users.FirstOrDefaultAsync(u => u.IsExternal && u.Email == emailDto.Email);

        if (userWithThisEmail != null)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailAlreadyTaken
            };

        if (user.Email == emailDto.Email)
            return new Result
            {
                Succeeded = false,
                Message = Results.RequestedEmailSameAsCurrent
            };

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, emailDto.Email);
        var link = AddTokenToUrl(Links.EmailChangeLink, token);

        user.RequestedEmail = emailDto.Email;
        await _userManager.UpdateAsync(user);

        await _sender.SendEmailAsync(emailDto.Email, Emails.EmailChangeSubject,
            Emails.EmailChangeMessage(link));

        return new Result
        {
            Message = Results.EmailChangeRequested
        };
    }

    public async Task<Result> ChangeEmailAsync(string? userId, string token)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        if (user.RequestedEmail == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.RequestedEmailNotFound
            };

        var changeResult = await _userManager.ChangeEmailAsync(user, user.RequestedEmail, token);

        return new Result
        {
            Succeeded = changeResult.Succeeded,
            Message = Result.IdentityResultsToMessage(changeResult) ?? Results.UserEmailChanged
        };
    }

    private static string AddTokenToUrl(string baseUrl, string token)
    {
        var uriBuilder = new UriBuilder(baseUrl);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["token"] = token;

        uriBuilder.Query = query.ToString();

        var modifiedUrl = uriBuilder.ToString();

        return modifiedUrl;
    }
}