using System.ComponentModel;

namespace MessengerApp.Application.Dtos.Channel;

public sealed class ChannelInfoDto
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [DisplayName("Chat picture")] public byte[]? ChatPictureBytes { get; set; }
}