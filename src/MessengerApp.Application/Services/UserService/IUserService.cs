using MessengerApp.Application.Dtos;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.UserService;

public interface IUserService
{
    public Task<Result<UserDto>> GetUserAsync(string userId);

    public Task<Result> UpdateUserProfileAsync(string userId, UserProfileDto profileDto);

    public Task<Result> ChangeEmailAsync(string userId, UserEmailDto emailDto);

    public Task<Result> ChangePasswordAsync(string userId, UserPasswordDto passwordDto);
}