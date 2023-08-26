using MessengerApp.Domain.Abstractions;
using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class DirectReaction
    : Reaction<Direct, DirectMessage, DirectAttachment, DirectReaction>
{
}