using MessengerApp.Application.Dtos.User;

namespace MessengerApp.Application.Dtos;

public sealed class MessageDto
{
    public string Id { get; set; } = null!;
    
    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public UserPreviewDto Sender { get; set; } = null!;
    
    public IEnumerable<AttachmentDto>? Attachments { get; set; }

    public IEnumerable<ReactionDto>? Reactions { get; set; }
}