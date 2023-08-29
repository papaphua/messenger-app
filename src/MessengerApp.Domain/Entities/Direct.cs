using MessengerApp.Domain.Abstractions.Chat;

namespace MessengerApp.Domain.Entities;

public sealed class Direct
    : Chat<Direct, DirectMessage, DirectAttachment, DirectReaction>
{
}