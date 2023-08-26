namespace MessengerApp.Application.Dtos.Group;

public sealed class GroupDto
{
    public string Id { get; set; } = null!;

    public GroupInfoDto GroupInfoDto { get; set; } = null!;

    public IEnumerable<Domain.Entities.User> Users { get; set; } = null!;

    public IEnumerable<Domain.Entities.User> Admins { get; set; } = null!;
}