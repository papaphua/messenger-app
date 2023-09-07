using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.GroupService;

public interface IGroupService
{
    Task<Result<GroupDto>> GetGroupAsync(string userId, string groupId);
    Task<Result<IEnumerable<GroupPreviewDto>>> GetGroupPreviewsAsync(string userId);
    Task<Result<GroupDto>> CreateGroupAsync(string userId, GroupInfoDto groupInfoDto);
    Task<Result<IEnumerable<GroupPreviewDto>>> FindGroupsByTitleAsync(string? search);
    Task<Result> LeaveGroupAsync(string userId, string groupId);
    Task<Result> CreateGroupMessageAsync(string userId, string groupId, CreateMessageDto createMessageDto);
}