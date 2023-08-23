using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class ChannelUser : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string ChannelId { get; set; } = null!;

    public string UserId { get; set; } = null!;
}