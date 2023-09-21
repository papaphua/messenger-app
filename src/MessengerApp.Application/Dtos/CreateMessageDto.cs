namespace MessengerApp.Application.Dtos;

public sealed class CreateMessageDto
{
    public string Content { get; set; } = null!;
    public IReadOnlyList<byte[]>? Attachments { get; set; }
}