namespace MessengerApp.Application.Dtos;

public sealed class CreateMessageDto
{
    public string Content { get; set; } = null!;
    public IEnumerable<byte[]>? Attachments { get; set; }
}