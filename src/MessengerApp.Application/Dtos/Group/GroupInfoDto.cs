namespace MessengerApp.Application.Dtos.Group;

public sealed class GroupInfoDto
{
    public string Title { get; set; } = null!;
    
    public string? Description { get; set; }

    public byte[]? ChatPictureBytes { get; set; }
}