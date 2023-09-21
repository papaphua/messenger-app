using MessengerApp.Domain.Abstractions.Chat;
using MessengerApp.Domain.Abstractions.Entities;

namespace MessengerApp.Domain.Entities;

public sealed class Group
    : Chat<Group, GroupMessage, GroupAttachment, GroupReaction>, IAdministratedChat, ICommentableChat, IReactableChat
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? ChatPictureBytes { get; set; }

    public bool IsPrivate { get; set; }

    public bool AllowComments { get; set; } = true;

    public bool AllowReactions { get; set; } = true;
}