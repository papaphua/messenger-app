namespace MessengerApp.Domain.Entities.Joints;

public sealed class ChannelAdmin
{
    public Guid ChannelId { get; set; }

    public Guid AdminId { get; set; }
}