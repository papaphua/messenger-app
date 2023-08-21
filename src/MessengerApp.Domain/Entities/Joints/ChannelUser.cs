namespace MessengerApp.Domain.Entities.Joints;

public sealed class ChannelUser
{
    public Guid ChannelId { get; set; }
    
    public Guid UserId { get; set; }
}