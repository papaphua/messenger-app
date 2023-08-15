using System.Web;
using AutoMapper;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Abstractions.Services;
using MessengerApp.Application.Dtos;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Application.Services.UserService;

public sealed class UserService : IUserService
{
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager, IMapper mapper, IEmailService emailService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _emailService = emailService;
    }


    public async Task<Result<UserDto>> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<UserDto>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        return new Result<UserDto>
        {
            Data = _mapper.Map<User, UserDto>(user)
        };
    }

    public async Task<Result> UpdateUserProfileAsync(string userId, UserProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        _mapper.Map(dto, user);
        var updateResult = await _userManager.UpdateAsync(user);

        return new Result
        {
            Succeeded = updateResult.Succeeded,
            Message = Result.IdentityResultsToMessage(updateResult) ?? Results.UserProfileUpdated
        };
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordDto passwordDto)
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

        await _emailService.SendEmailAsync(user.Email, Emails.EmailConfirmationSubject,
            Emails.EmailConfirmationMessage(link));

        return new Result
        {
            Message = Results.EmailConfirmationRequested
        };
    }

    public async Task<Result> ConfirmEmailAsync(string? userId, string token)
    {
        if (userId == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailConfirmationNotAuthenticated
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

        var confirmationResult = await _userManager.ConfirmEmailAsync(user, token);

        return new Result
        {
            Succeeded = confirmationResult.Succeeded,
            Message = Result.IdentityResultsToMessage(confirmationResult) ?? Results.UserEmailConfirmed
        };
    }

    public async Task<Result> RequestEmailChangeAsync(string userId, UserEmailDto emailDto)
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
                Message = Results.ExternalUser
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

        await _emailService.SendEmailAsync(emailDto.Email, Emails.EmailChangeSubject,
            Emails.EmailChangeMessage(link));

        return new Result
        {
            Message = Results.EmailChangeRequested
        };
    }

    public async Task<Result> ChangeEmailAsync(string? userId, string token)
    {
        if (userId == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.EmailChangeNotAuthenticated
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