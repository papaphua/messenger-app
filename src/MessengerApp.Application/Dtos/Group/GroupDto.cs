namespace MessengerApp.Application.Dtos.Group;

public sealed class GroupDto
{
    public string Id { get; set; } = null!;

    public GroupInfoDto GroupInfoDto { get; set; } = null!;

    public IReadOnlyList<MessageDto> Messages { get; set; } = null!;
}