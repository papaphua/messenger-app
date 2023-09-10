namespace MessengerApp.Application.Dtos.Channel;

public sealed class ChannelCommentsDto
{
    public MessageDto Message { get; set; } = null!;

    public IEnumerable<CommentDto> Comments { get; set; } = null!;
}