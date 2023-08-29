using MessengerApp.Application.Dtos.Profile;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.ProfileService;

public interface IProfileService
{
    public Task<Result<ProfileDto>> GetProfileAsync(string userId);

    public Task<Result> UpdateProfileInfoAsync(string userId, ProfileInfoDto profileInfoDto);

    public Task<Result> UpdateProfilePictureAsync(string userId, byte[] profilePictureBytes);

    public Task<Result> ChangePasswordAsync(string userId, PasswordDto passwordDto);

    public Task<Result> RequestEmailConfirmationAsync(string userId);

    public Task<Result> ConfirmEmailAsync(string userId, string token);

    public Task<Result> RequestEmailChangeAsync(string userId, ProfileEmailDto profileEmailDto);

    public Task<Result> ChangeEmailAsync(string userId, string token);
}