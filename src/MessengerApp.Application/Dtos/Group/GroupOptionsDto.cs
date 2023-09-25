namespace MessengerApp.Application.Dtos.Group;

public sealed class GroupOptionsDto
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? ChatPictureBytes { get; set; }
    
    public bool IsPrivate { get; set; }
    
    public bool AllowReactions { get; set; }
}