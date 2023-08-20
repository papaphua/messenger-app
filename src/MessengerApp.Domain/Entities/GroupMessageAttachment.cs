using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class GroupMessageAttachment
    : Attachment<Group, GroupMessage, GroupMessageAttachment, GroupMessageReaction>
{
}