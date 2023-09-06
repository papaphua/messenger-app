using MessengerApp.Domain.Entities;

namespace MessengerApp.Application.Dtos.Channel;

public sealed class ChannelDto
{
    public string Id { get; set; } = null!;

    public ChannelInfoDto ChannelInfoDto { get; set; } = null!;
    
    public IEnumerable<MessageDto> Messages { get; set; } = null!;
}