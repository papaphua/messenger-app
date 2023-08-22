using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class ChannelUser : IEntity
{
    public Guid ChannelId { get; set; }

    public Guid UserId { get; set; }
    
    public Guid Id { get; set; }
}