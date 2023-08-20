using MessengerApp.Domain.Entities;

namespace MessengerApp.Domain.Abstractions;

public abstract class Reaction<TChat, TMessage, TAttachment, TReaction> : IEntity
    where TMessage : Message<TChat, TMessage, TAttachment, TReaction>
    where TChat : Chat<TChat, TMessage, TAttachment, TReaction>
    where TAttachment : Attachment<TChat, TMessage, TAttachment, TReaction>
    where TReaction : Reaction<TChat, TMessage, TAttachment, TReaction>
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public Guid MessageId { get; set; }

    public TMessage Message { get; set; } = null!;

    public string Content { get; set; } = null!;

    public Guid Id { get; set; } = Guid.NewGuid();
}