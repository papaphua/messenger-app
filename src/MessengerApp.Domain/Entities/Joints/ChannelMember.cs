using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class ChannelMember : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string ChannelId { get; set; } = null!;

    public string MemberId { get; set; } = null!;
    
    public bool IsOwner { get; set; }

    public bool IsAdmin { get; set; }
    
    public static ChannelMember AddMemberToChannel(string channelId, string userId)
    {
        return new ChannelMember
        {
            ChannelId = channelId,
            MemberId = userId
        };
    }

    public static ChannelMember AddAdminToChannel(string channelId, string userId)
    {
        return new ChannelMember
        {
            ChannelId = channelId,
            MemberId = userId,
            IsAdmin = true
        };
    }
    
    public static ChannelMember AddOwnerToChannel(string channelId, string userId)
    {
        return new ChannelMember
        {
            ChannelId = channelId,
            MemberId = userId,
            IsOwner = true
        };
    }
}