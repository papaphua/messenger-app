using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Domain.Enumerations;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.ChannelService;

public interface IChannelService
{
    Task<Result<ChannelDto>> GetChannelAsync(string userId, string channelId);
    Task<Result<IReadOnlyList<ChannelPreviewDto>>> GetChannelPreviewsAsync(string userId);
    Task<Result<ChannelDto>> CreateChannelAsync(string userId, ChannelInfoDto channelInfoDto);
    Task<Result<ChannelDto>> JoinChannelAsync(string userId, string channelId);
    Task<Result> LeaveChannelAsync(string userId, string channelId);
    Task<Result> CreateChannelMessageAsync(string userId, string channelId, CreateMessageDto createMessageDto);
    Task<Result<string>> CreateChannelReactionAsync(string userId, string messageId, Reaction reaction);
    Task<Result<ChannelCommentsDto>> GetChannelMessageCommentsAsync(string userId, string messageId);
    Task<Result> CreateChannelMessageCommentAsync(string userId, string messageId, CreateCommentDto createCommentDto);
}