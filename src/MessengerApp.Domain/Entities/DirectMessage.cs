using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class DirectMessage
    : Message<Direct, DirectMessage, DirectAttachment, DirectReaction>
{
}