using System.ComponentModel;

namespace MessengerApp.Application.Dtos.Group;

public sealed class GroupInfoDto
{
    public string Title { get; set; } = null!;
    
    public string? Description { get; set; }

    [DisplayName("Chat picture")]
    public byte[]? ChatPictureBytes { get; set; }
}