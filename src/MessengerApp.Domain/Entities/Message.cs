using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Message : IEntity
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime DateTimeCreated { get; set; }

    public ICollection<Attachment> Attachments { get; set; } = null!;

    public ICollection<Reaction> Reactions { get; set; } = null!;
}