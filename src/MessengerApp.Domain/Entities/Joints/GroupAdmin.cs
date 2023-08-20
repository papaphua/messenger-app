namespace MessengerApp.Domain.Entities.Joints;

public sealed class GroupAdmin
{
    public Guid GroupId { get; set; }

    public Guid AdminId { get; set; }
}