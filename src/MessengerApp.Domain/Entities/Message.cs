using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Message : Entity
{
    public Message(Guid id)
        : base(id)
    {
        DateTimeCreated = DateTime.UtcNow;
    }

    public required Guid UserId { get; set; }

    public required User User { get; set; }
    
    public required Guid ChatId { get; set; }
    
    public required Chat Chat { get; set; }

    public required string Content { get; set; }

    public required DateTime DateTimeCreated { get; set; }

    public ICollection<Attachment> Attachments { get; set; } = null!;

    public ICollection<Reaction> Reactions { get; set; } = null!;
}