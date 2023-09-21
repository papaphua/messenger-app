using MessengerApp.Domain.Entities;

namespace MessengerApp.Domain.Abstractions.Entities;

public abstract class Reaction<TChat, TMessage, TAttachment, TReaction> : IEntity
    where TMessage : Message<TChat, TMessage, TAttachment, TReaction>
    where TChat : Chat<TChat, TMessage, TAttachment, TReaction>
    where TAttachment : Attachment<TChat, TMessage, TAttachment, TReaction>
    where TReaction : Reaction<TChat, TMessage, TAttachment, TReaction>
{
    public int ReactionNum { get; set; }

    public User User { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public TMessage Message { get; set; } = null!;

    public string MessageId { get; set; } = null!;
    public string Id { get; set; } = Guid.NewGuid().ToString();
}