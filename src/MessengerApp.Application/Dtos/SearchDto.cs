using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Dtos.User;

namespace MessengerApp.Application.Dtos;

public sealed class SearchDto
{
    public IReadOnlyList<UserPreviewDto>? Users { get; set; }
    public IReadOnlyList<GroupPreviewDto>? Groups { get; set; }
    public IReadOnlyList<ChannelPreviewDto>? Channels { get; set; }
}