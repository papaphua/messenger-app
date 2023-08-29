using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class DirectAttachment
    : Attachment<Direct, DirectMessage, DirectAttachment, DirectReaction>
{
}