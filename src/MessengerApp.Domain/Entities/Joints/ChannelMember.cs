using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class ChannelMember : IEntity
{
    public string ChannelId { get; set; } = null!;

    public string MembersId { get; set; } = null!;

    public bool IsOwner { get; set; }

    public bool IsAdmin { get; set; }
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public static ChannelMember AddMemberToChannel(string channelId, string userId)
    {
        return new ChannelMember
        {
            ChannelId = channelId,
            MembersId = userId
        };
    }

    public static ChannelMember AddAdminToChannel(string channelId, string userId)
    {
        return new ChannelMember
        {
            ChannelId = channelId,
            MembersId = userId,
            IsAdmin = true
        };
    }

    public static ChannelMember AddOwnerToChannel(string channelId, string userId)
    {
        return new ChannelMember
        {
            ChannelId = channelId,
            MembersId = userId,
            IsOwner = true
        };
    }
}