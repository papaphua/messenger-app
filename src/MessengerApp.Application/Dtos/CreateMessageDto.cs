using System.ComponentModel.DataAnnotations;

namespace MessengerApp.Application.Dtos;

public sealed class CreateMessageDto
{
    [Required(ErrorMessage = "MessageRequired")]
    public string Content { get; set; } = null!;
    public IReadOnlyList<byte[]>? Attachments { get; set; }
}