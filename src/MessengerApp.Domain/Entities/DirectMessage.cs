using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public sealed class DirectMessage
    : Message<Direct, DirectMessage, DirectAttachment, DirectReaction>
{
}