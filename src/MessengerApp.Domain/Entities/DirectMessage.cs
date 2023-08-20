using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class DirectMessage
    : Message<Direct, DirectMessage, DirectMessageAttachment, DirectMessageReaction>
{
}