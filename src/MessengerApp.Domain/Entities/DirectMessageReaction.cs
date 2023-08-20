using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class DirectMessageReaction
    : Reaction<Direct, DirectMessage, DirectMessageAttachment, DirectMessageReaction>
{
}