using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class DirectMember : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string DirectId { get; set; } = null!;

    public string MemberId { get; set; } = null!;
    
    public static DirectMember AddMemberToDirect(string directId, string userId)
    {
        return new DirectMember
        {
            DirectId = directId,
            MemberId = userId
        };
    }
}