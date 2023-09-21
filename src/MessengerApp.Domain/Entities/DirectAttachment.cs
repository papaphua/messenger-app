using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public sealed class DirectAttachment
    : Attachment<Direct, DirectMessage, DirectAttachment, DirectReaction>
{
}