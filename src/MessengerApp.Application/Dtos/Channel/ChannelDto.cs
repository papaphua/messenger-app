namespace MessengerApp.Application.Dtos.Channel;

public sealed class ChannelDto
{
    public string Id { get; set; } = null!;

    public ChannelInfoDto ChannelInfoDto { get; set; } = null!;
}