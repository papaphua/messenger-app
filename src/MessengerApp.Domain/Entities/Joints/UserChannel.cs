namespace MessengerApp.Domain.Entities.Joints;

public sealed class UserChannel
{
    public Guid UserId { get; set; }

    public Guid ChannelId { get; set; }
}