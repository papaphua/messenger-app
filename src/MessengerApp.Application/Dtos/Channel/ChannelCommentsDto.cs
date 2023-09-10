namespace MessengerApp.Application.Dtos.Channel;

public sealed class ChannelCommentsDto
{
    public string MessageId { get; set; } = null!;

    public IEnumerable<CommentDto> Comments { get; set; } = null!;
}