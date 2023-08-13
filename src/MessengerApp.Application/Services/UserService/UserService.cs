using AutoMapper;
using MessengerApp.Application.Dtos;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Application.Services.UserService;

public sealed class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
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

    public async Task<Result> ChangeEmailAsync(string userId, UserEmailDto emailDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        if (user.PasswordHash == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserWithExternalLogin
            };


        // TODO need token
        var emailResult = await _userManager.ChangeEmailAsync(user, emailDto.Email, "");

        return new Result
        {
            Succeeded = emailResult.Succeeded,
            Message = Result.IdentityResultsToMessage(emailResult) ?? Results.UserEmailUpdated
        };
    }

    public async Task<Result> ChangePasswordAsync(string userId, UserPasswordDto passwordDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        if (user.PasswordHash == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserWithExternalLogin
            };

        var passwordResult =
            await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

        return new Result
        {
            Succeeded = passwordResult.Succeeded,
            Message = Result.IdentityResultsToMessage(passwordResult) ?? Results.UserPasswordUpdated
        };
    }
}