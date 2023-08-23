using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Direct
    : Chat<Direct, DirectMessage, DirectMessageAttachment, DirectMessageReaction>
{
}