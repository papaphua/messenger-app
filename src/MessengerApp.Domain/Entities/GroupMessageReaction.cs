using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public class GroupMessageReaction
    : Reaction<Group, GroupMessage, GroupMessageAttachment, GroupMessageReaction>
{
}