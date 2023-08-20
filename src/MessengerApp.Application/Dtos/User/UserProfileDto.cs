namespace MessengerApp.Application.Dtos.User;

public sealed class UserProfileDto
{
    public UserInfoDto UserInfoDto { get; set; } = null!;

    public UserEmailDto UserEmailDto { get; set; } = null!;

    public UserProfilePictureDto UserProfilePictureDto { get; set; } = null!;
}