using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Comment : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public User Sender { get; set; } = null!;

    public string SenderId { get; set; } = null!;

    public ChannelMessage Message { get; set; } = null!;

    public string MessageId { get; set; } = null!;
}