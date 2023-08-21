namespace MessengerApp.Application.Dtos.Direct;

public sealed class DirectPreviewDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public byte[]? ProfilePictureBytes { get; set; }
}