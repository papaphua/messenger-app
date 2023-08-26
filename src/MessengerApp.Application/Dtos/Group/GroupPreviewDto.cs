namespace MessengerApp.Application.Dtos.Group;

public sealed class GroupPreviewDto
{
    public string Id { get; set; } = null!;
    
    public string Title { get; set; } = null!;

    public byte[]? ChatPictureBytes { get; set; }
}