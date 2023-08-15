using MessengerApp.Application.Dtos;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.UserService;

public interface IUserService
{
    public Task<Result<UserDto>> GetUserAsync(string userId);

    public Task<Result> UploadProfilePictureAsync(string userId, byte[] pictureBytes);

    public Task<Result> UpdateUserProfileAsync(string userId, UserProfileDto profileDto);

    public Task<Result> ChangePasswordAsync(string userId, ChangePasswordDto passwordDto);

    public Task<Result> RequestEmailConfirmationAsync(string userId);

    public Task<Result> ConfirmEmailAsync(string userId, string token);

    public Task<Result> RequestEmailChangeAsync(string userId, UserEmailDto emailDto);

    public Task<Result> ChangeEmailAsync(string userId, string token);
}