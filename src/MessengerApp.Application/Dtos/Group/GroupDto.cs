using MessengerApp.Domain.Entities;

namespace MessengerApp.Application.Dtos.Group;

public sealed class GroupDto
{
    public string Id { get; set; } = null!;

    public GroupInfoDto GroupInfoDto { get; set; } = null!;
    
    public IEnumerable<GroupMessage> Messages { get; set; } = null!;
}