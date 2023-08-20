using MessengerApp.Domain.Entities;

namespace MessengerApp.Domain.Abstractions;

public abstract class Message<TChat, TMessage, TAttachment, TReaction> : IEntity
    where TMessage : Message<TChat, TMessage, TAttachment, TReaction>
    where TChat : Chat<TChat, TMessage, TAttachment, TReaction>
    where TAttachment : Attachment<TChat, TMessage, TAttachment, TReaction>
    where TReaction : Reaction<TChat, TMessage, TAttachment, TReaction>
{
    public Guid SenderId { get; set; }

    public User Sender { get; set; } = null!;

    public Guid ChatId { get; set; }

    public TChat Chat { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ICollection<TAttachment> Attachments { get; set; } = new List<TAttachment>();

    public ICollection<TReaction> Reactions { get; set; } = new List<TReaction>();

    public Guid Id { get; set; } = Guid.NewGuid();
}