using MessengerApp.Domain.Abstractions.Chat;
using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public sealed class Direct
    : Chat<Direct, DirectMessage, DirectAttachment, DirectReaction>, IReactableChat
{
    public bool AllowReactions { get; set; } = true;
}