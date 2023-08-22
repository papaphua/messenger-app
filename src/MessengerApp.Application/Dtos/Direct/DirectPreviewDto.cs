namespace MessengerApp.Application.Dtos.Direct;

public sealed class DirectPreviewDto
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public byte[]? ProfilePictureBytes { get; set; }
}