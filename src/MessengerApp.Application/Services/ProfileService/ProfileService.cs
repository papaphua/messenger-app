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
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IEmailSender _sender;
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
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<ProfileDto>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;
        var profileDto = _mapper.Map<User, ProfileDto>(user);
        
        return new Result<ProfileDto>
        {
            Data = profileDto
        };
    }

    public async Task<Result> UploadProfilePictureAsync(string? userId, byte[] profilePictureBytes)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        user.ProfilePictureBytes = profilePictureBytes;
        await _userManager.UpdateAsync(user);

        return new Result
        {
            Message = Results.ProfilePictureUpdated
        };
    }

    public async Task<Result> UpdateUserInfoAsync(string? userId, ProfileInfoDto profileInfoDto)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;
        
        _mapper.Map(profileInfoDto, user);
        var updateResult = await _userManager.UpdateAsync(user);

        return new Result
        {
            Succeeded = updateResult.Succeeded,
            Message = Result.IdentityResultsToString(updateResult) ?? Results.UserProfileUpdated
        };
    }

    public async Task<Result> ChangePasswordAsync(string? userId, ChangePasswordDto changePasswordDto)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        if (user.IsExternal)
            return new Result
            {
                Succeeded = false,
                Message = Results.ExternalUser
            };

        var changePasswordResult =
            await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

        return new Result
        {
            Succeeded = changePasswordResult.Succeeded,
            Message = Result.IdentityResultsToString(changePasswordResult) ?? Results.UserPasswordChanged
        };
    }

    public async Task<Result> RequestEmailConfirmationAsync(string? userId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

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
        var link = AddTokenToUrl(Urls.EmailConfirmationLink, token);

        await _sender.SendEmailAsync(user.Email!, Emails.ConfirmationSubject,
            Emails.GetConfirmationMessage(link));

        return new Result
        {
            Message = Results.EmailConfirmationRequested
        };
    }

    public async Task<Result> ConfirmEmailAsync(string? userId, string token)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        if (user.EmailConfirmed)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailAlreadyConfirmed
            };

        var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, token);

        return new Result
        {
            Succeeded = confirmEmailResult.Succeeded,
            Message = Result.IdentityResultsToString(confirmEmailResult) ?? Results.UserEmailConfirmed
        };
    }

    public async Task<Result> RequestEmailChangeAsync(string? userId, ProfileEmailDto profileEmailDto)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        if (user.IsExternal)
            return new Result
            {
                Succeeded = false,
                Message = Results.ExternalUser
            };

        var userWithSameEmail =
            await _userManager.Users.FirstOrDefaultAsync(u => u.IsExternal && u.Email == profileEmailDto.Email);

        if (userWithSameEmail != null)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailAlreadyTaken
            };

        if (user.Email == profileEmailDto.Email)
            return new Result
            {
                Succeeded = false,
                Message = Results.RequestedEmailSameAsCurrent
            };

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, profileEmailDto.Email);
        var link = AddTokenToUrl(Urls.EmailChangeLink, token);

        user.RequestedEmail = profileEmailDto.Email;
        await _userManager.UpdateAsync(user);

        await _sender.SendEmailAsync(profileEmailDto.Email, Emails.ChangeSubject,
            Emails.GetChangeMessage(link));

        return new Result
        {
            Message = Results.EmailChangeRequested
        };
    }

    public async Task<Result> ChangeEmailAsync(string? userId, string token)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        if (user.RequestedEmail == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.RequestedEmailNotFound
            };

        var changeEmailResult = await _userManager.ChangeEmailAsync(user, user.RequestedEmail, token);

        return new Result
        {
            Succeeded = changeEmailResult.Succeeded,
            Message = Result.IdentityResultsToString(changeEmailResult) ?? Results.UserEmailChanged
        };
    }

    private static string AddTokenToUrl(string url, string token)
    {
        var uriBuilder = new UriBuilder(url);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["token"] = token;

        uriBuilder.Query = query.ToString();

        var modifiedUrl = uriBuilder.ToString();

        return modifiedUrl;
    }
}