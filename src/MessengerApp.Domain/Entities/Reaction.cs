using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Reaction : IEntity
{
    public Guid Id { get; set; }

    public required Guid UserId { get; set; }

    public required User User { get; set; }
    
    public required Guid MessageId { get; set; }
    
    public required Message Message { get; set; }

    public required string Content { get; set; }
}