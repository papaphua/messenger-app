using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class GroupMessage
    : Message<Group, GroupMessage, GroupMessageAttachment, GroupMessageReaction>
{
}