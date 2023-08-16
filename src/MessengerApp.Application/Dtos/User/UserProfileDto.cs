namespace MessengerApp.Application.Dtos.User;

public sealed class UserProfileDto
{
    public required UserInfoDto UserInfoDto { get; set; }
    
    public required UserEmailDto UserEmailDto { get; set; }
    
    public required UserProfilePictureDto UserProfilePictureDto { get; set; }
}