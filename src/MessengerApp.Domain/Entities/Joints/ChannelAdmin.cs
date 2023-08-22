using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class ChannelAdmin : IEntity
{
    public Guid ChannelId { get; set; }

    public Guid AdminId { get; set; }
    
    public Guid Id { get; set; }
}