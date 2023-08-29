using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class GroupMember : IEntity
{
    public string GroupId { get; set; } = null!;

    public string MembersId { get; set; } = null!;

    public bool IsOwner { get; set; }

    public bool IsAdmin { get; set; }
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public static GroupMember AddMemberToGroup(string groupId, string userId)
    {
        return new GroupMember
        {
            GroupId = groupId,
            MembersId = userId
        };
    }

    public static GroupMember AddAdminToGroup(string groupId, string userId)
    {
        return new GroupMember
        {
            GroupId = groupId,
            MembersId = userId,
            IsAdmin = true
        };
    }

    public static GroupMember AddOwnerToGroup(string groupId, string userId)
    {
        return new GroupMember
        {
            GroupId = groupId,
            MembersId = userId,
            IsOwner = true
        };
    }
}