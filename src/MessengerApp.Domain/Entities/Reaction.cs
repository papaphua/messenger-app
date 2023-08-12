using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Reaction : Entity
{
    public Reaction(Guid id)
        : base(id)
    {
    }

    public required Guid UserId { get; set; }

    public required User User { get; set; }
    
    public required Guid MessageId { get; set; }
    
    public required Message Message { get; set; }

    public required string Content { get; set; }
}