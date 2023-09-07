using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Dtos.User;

namespace MessengerApp.Application.Dtos;

public sealed class SearchDto
{
    public IEnumerable<UserPreviewDto>? Users { get; set; }
    public IEnumerable<GroupPreviewDto>? Groups { get; set; }
    public IEnumerable<ChannelPreviewDto>? Channels { get; set; }
}