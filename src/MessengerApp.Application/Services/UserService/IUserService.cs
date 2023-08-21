using MessengerApp.Application.Dtos.User;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.UserService;

public interface IUserService
{
    public Task<Result<UserProfileDto>> GetUserProfileAsync(string? userId);

    public Task<Result> UploadProfilePictureAsync(string? userId, UserProfilePictureDto profilePictureDto);

    public Task<Result> UpdateUserInfoAsync(string? userId, UserInfoDto infoDto);

    public Task<Result> ChangePasswordAsync(string? userId, ChangePasswordDto passwordDto);

    public Task<Result> RequestEmailConfirmationAsync(string? userId);

    public Task<Result> ConfirmEmailAsync(string? userId, string token);

    public Task<Result> RequestEmailChangeAsync(string? userId, UserEmailDto emailDto);

    public Task<Result> ChangeEmailAsync(string? userId, string token);

    public Task<Result<User>> DoesUserExist(string? userId);
}