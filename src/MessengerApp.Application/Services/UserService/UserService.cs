using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace MessengerApp.Application.Services.UserService;

public sealed class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<User>> DoesUserExist(string? userId)
    {
        if (userId == null)
            return new Result<User>
            {
                Succeeded = false,
                Message = Results.UserNotAuthenticated
            };

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<User>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        return new Result<User>
        {
            Data = user
        };
    }
}