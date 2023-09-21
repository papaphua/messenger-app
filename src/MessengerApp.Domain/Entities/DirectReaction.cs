using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public sealed class DirectReaction
    : Reaction<Direct, DirectMessage, DirectAttachment, DirectReaction>
{
}