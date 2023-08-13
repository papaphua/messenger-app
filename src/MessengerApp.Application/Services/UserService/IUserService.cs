using MessengerApp.Application.Dtos;

namespace MessengerApp.Application.Services.UserService;

public interface IUserService
{
    public Task<UserDto> GetUserAsync(string userId);

    public Task UpdateUserProfileAsync(string userId, UserProfileDto profileDto);

    public Task ChangeEmailAsync(string userId, UserEmailDto emailDto);

    public Task ChangePasswordAsync(string userId, UserPasswordDto passwordDto);
}