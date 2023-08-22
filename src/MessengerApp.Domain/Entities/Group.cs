using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Group
    : Chat<Group, GroupMessage, GroupMessageAttachment, GroupMessageReaction>, IAdministratedChat
{
    public string? Description { get; set; }

    public byte[]? ChatPicture { get; set; }

    public ICollection<User> Admins { get; set; } = null!;
}