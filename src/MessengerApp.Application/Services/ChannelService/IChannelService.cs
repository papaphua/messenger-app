using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.ChannelService;

public interface IChannelService
{
    Task<Result<ChannelDto>> GetChannelAsync(string userId, string channelId);
    Task<Result<IEnumerable<ChannelPreviewDto>>> GetChannelPreviewsAsync(string userId);
    Task<Result<ChannelDto>> CreateChannelAsync(string userId, ChannelInfoDto channelInfoDto);
    Task<Result> LeaveChannelAsync(string userId, string channelId);
}