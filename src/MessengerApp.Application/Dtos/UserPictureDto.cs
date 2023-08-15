using System.ComponentModel;

namespace MessengerApp.Application.Dtos;

public class UserPictureDto
{
    [DisplayName("Profile picture")]
    public byte[]? ProfilePicture { get; set; }
}