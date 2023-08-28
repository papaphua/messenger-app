namespace MessengerApp.Application.Dtos.Channel;

public sealed class ChannelPreviewDto
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public byte[]? ChatPictureBytes { get; set; }
}