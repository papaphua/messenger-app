namespace MessengerApp.Domain.Abstractions.Chat;

public abstract class Attachment<TChat, TMessage, TAttachment, TReaction> : IEntity
    where TMessage : Message<TChat, TMessage, TAttachment, TReaction>
    where TChat : Chat<TChat, TMessage, TAttachment, TReaction>
    where TAttachment : Attachment<TChat, TMessage, TAttachment, TReaction>
    where TReaction : Reaction<TChat, TMessage, TAttachment, TReaction>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public byte[] ContentBytes { get; set; } = null!;

    public TMessage Message { get; set; } = null!;

    public string MessageId { get; set; } = null!;
}