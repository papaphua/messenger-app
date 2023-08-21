namespace MessengerApp.Domain.Entities.Joints;

public sealed class GroupUser
{
    public Guid GroupId { get; set; }
    
    public Guid UserId { get; set; }
}