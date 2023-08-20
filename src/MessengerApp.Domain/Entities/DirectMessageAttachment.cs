using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class DirectMessageAttachment
    : Attachment<Direct, DirectMessage, DirectMessageAttachment, DirectMessageReaction>
{
}