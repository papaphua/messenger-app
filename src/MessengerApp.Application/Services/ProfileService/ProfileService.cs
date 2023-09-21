using System.Web;
using AutoMapper;
using MessengerApp.Application.Dtos.Profile;
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

    public ProfileService(UserManager<User> userManager, IMapper mapper, IEmailSender sender)
    {
        _userManager = userManager;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task<Result<ProfileDto>> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<ProfileDto>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var profileDto = _mapper.Map<User, ProfileDto>(user);

        return new Result<ProfileDto>
        {
            Data = profileDto
        };
    }

    public async Task<Result> UpdateProfileInfoAsync(string userId, ProfileInfoDto profileInfoDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        _mapper.Map(profileInfoDto, user);

        var updateResult = await _userManager.UpdateAsync(user);

        return new Result
        {
            Succeeded = updateResult.Succeeded,
            Message = Result.IdentityResultToString(updateResult) ?? Results.ProfileUpdated
        };
    }

    public async Task<Result> UpdateProfilePictureAsync(string userId, byte[] profilePictureBytes)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        user.ProfilePictureBytes = profilePictureBytes;

        await _userManager.UpdateAsync(user);

        return new Result
        {
            Message = Results.ProfilePictureUpdate
        };
    }

    public async Task<Result> ChangePasswordAsync(string userId, PasswordDto passwordDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        if (user.IsExternal)
            return new Result
            {
                Succeeded = false,
                Message = Results.ExternalUserPasswordError
            };

        var changePasswordResult =
            await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

        return new Result
        {
            Succeeded = changePasswordResult.Succeeded,
            Message = Result.IdentityResultToString(changePasswordResult) ?? Results.PasswordChanged
        };
    }

    public async Task<Result> RequestEmailConfirmationAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        if (user.IsExternal)
            return new Result
            {
                Succeeded = false,
                Message = Results.ExternalUserEmailError
            };

        if (user.EmailConfirmed)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailAlreadyConfirmed
            };

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = AddTokenToUrl(Urls.EmailConfirmationLink, token);

        await _sender.SendEmailAsync(user.Email!, Emails.EmailConfirmationSubject,
            Emails.EmailConfirmationMessage(link));

        return new Result
        {
            Message = Results.EmailConfirmationRequestSentTo(user.Email!)
        };
    }

    public async Task<Result> ConfirmEmailAsync(string userId, string? token)
    {
        if (token == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.InvalidLink
            };
        
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

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
            Message = Result.IdentityResultToString(confirmEmailResult) ?? Results.EmailConfirmed
        };
    }

    public async Task<Result> RequestEmailChangeAsync(string userId, ProfileEmailDto profileEmailDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        if (user.IsExternal)
            return new Result
            {
                Succeeded = false,
                Message = Results.ExternalUserEmailError
            };

        var userWithSameEmail =
            await _userManager.Users.FirstOrDefaultAsync(u => u.IsExternal && u.Email == profileEmailDto.Email);

        if (userWithSameEmail != null)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailAlreadyTaken(profileEmailDto.Email)
            };

        if (user.Email == profileEmailDto.Email)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailSameAsCurrent
            };

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, profileEmailDto.Email);
        var link = AddTokenToUrl(Urls.EmailChangeLink, token);

        user.RequestedEmail = profileEmailDto.Email;

        await _userManager.UpdateAsync(user);

        await _sender.SendEmailAsync(profileEmailDto.Email, Emails.EmailChangeSubject,
            Emails.EmailChangeMessage(link));

        return new Result
        {
            Message = Results.EmailChangeRequestSentTo(user.Email!)
        };
    }

    public async Task<Result> ChangeEmailAsync(string userId, string? token)
    {
        if (token == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.InvalidLink
            };
        
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };
        
        if (user.RequestedEmail == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailChangeError
            };
        
        var changeEmailResult = await _userManager.ChangeEmailAsync(user, user.RequestedEmail, token);

        return new Result
        {
            Succeeded = changeEmailResult.Succeeded,
            Message = Result.IdentityResultToString(changeEmailResult) ?? Results.EmailChanged
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