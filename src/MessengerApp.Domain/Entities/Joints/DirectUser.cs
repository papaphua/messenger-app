using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities.Joints;

public sealed class DirectUser : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string DirectId { get; set; } = null!;

    public string UserId { get; set; } = null!;
    
    public static DirectUser AddUserToDirect(string directId, string userId)
    {
        return new DirectUser
        {
            DirectId = directId,
            UserId = userId
        };
    }
}