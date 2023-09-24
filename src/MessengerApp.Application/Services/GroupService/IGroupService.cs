using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Domain.Enumerations;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.GroupService;

public interface IGroupService
{
    Task<Result<GroupDto>> GetGroupAsync(string userId, string groupId);
    Task<Result<IReadOnlyList<GroupPreviewDto>>> GetGroupPreviewsAsync(string userId);
    Task<Result<GroupDto>> CreateGroupAsync(string userId, GroupInfoDto groupInfoDto);
    Task<Result<GroupDto>> JoinGroupAsync(string userId, string groupId);
    Task<Result> LeaveGroupAsync(string userId, string groupId);
    Task<Result> CreateGroupMessageAsync(string userId, string groupId, CreateMessageDto createMessageDto);
    Task<Result<string>> CreateGroupReactionAsync(string userId, string messageId, Reaction reaction);
}