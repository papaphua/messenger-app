namespace MessengerApp.Domain.Abstractions;

public abstract class Attachment<TChat, TMessage, TAttachment, TReaction> : IEntity
    where TMessage : Message<TChat, TMessage, TAttachment, TReaction>
    where TChat : Chat<TChat, TMessage, TAttachment, TReaction>
    where TAttachment : Attachment<TChat, TMessage, TAttachment, TReaction>
    where TReaction : Reaction<TChat, TMessage, TAttachment, TReaction>
{
    public Guid MessageId { get; set; }

    public TMessage Message { get; set; } = null!;

    public byte[] ContentBytes { get; set; } = null!;

    public Guid Id { get; set; } = Guid.NewGuid();
}