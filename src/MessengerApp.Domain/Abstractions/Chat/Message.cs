using MessengerApp.Domain.Entities;

namespace MessengerApp.Domain.Abstractions.Chat;

public abstract class Message<TChat, TMessage, TAttachment, TReaction> : IEntity
    where TMessage : Message<TChat, TMessage, TAttachment, TReaction>
    where TChat : Chat<TChat, TMessage, TAttachment, TReaction>
    where TAttachment : Attachment<TChat, TMessage, TAttachment, TReaction>
    where TReaction : Reaction<TChat, TMessage, TAttachment, TReaction>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public User Sender { get; set; } = null!;

    public string SenderId { get; set; } = null!;

    public TChat Chat { get; set; } = null!;

    public string ChatId { get; set; } = null!;

    public ICollection<TAttachment> Attachments { get; set; } = null!;

    public ICollection<TReaction> Reactions { get; set; } = null!;
}