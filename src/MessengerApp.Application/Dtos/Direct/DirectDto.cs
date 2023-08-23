using MessengerApp.Application.Dtos.Profile;

namespace MessengerApp.Application.Dtos.Direct;

public sealed class DirectDto
{
    public string Id { get; set; } = null!;
    
    public ProfileInfoDto ProfileInfoDto { get; set; } = null!;
    
    public byte[]? ProfilePictureBytes { get; set; }
}