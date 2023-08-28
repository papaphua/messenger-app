using MessengerApp.Domain.Entities;

namespace MessengerApp.Domain.Abstractions.Chat;

public abstract class Chat<TChat, TMessage, TAttachment, TReaction> : IEntity
    where TMessage : Message<TChat, TMessage, TAttachment, TReaction>
    where TChat : Chat<TChat, TMessage, TAttachment, TReaction>
    where TAttachment : Attachment<TChat, TMessage, TAttachment, TReaction>
    where TReaction : Reaction<TChat, TMessage, TAttachment, TReaction>
{
    public ICollection<User> Members { get; set; } = null!;

    public ICollection<TMessage> Messages { get; set; } = null!;
    public string Id { get; set; } = Guid.NewGuid().ToString();
}