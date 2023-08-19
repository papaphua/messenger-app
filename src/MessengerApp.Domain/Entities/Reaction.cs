using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Reaction : IEntity
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public Guid MessageId { get; set; }

    public Message Message { get; set; } = null!;

    public string Content { get; set; } = null!;
}