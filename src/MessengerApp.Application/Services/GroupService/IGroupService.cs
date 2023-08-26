using MessengerApp.Application.Dtos.Group;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.GroupService;

public interface IGroupService
{
    Task<Result<GroupDto>> GetGroupAsync(string? userId, string groupId);
    Task<Result<IEnumerable<GroupPreviewDto>>> GetGroupPreviewsAsync(string? userId);
    Task<Result<string>> CreateGroupAsync(string? userId, GroupInfoDto groupInfoDto);
    Task<Result> AddGroupMemberAsync(string? userId, string groupId, string memberId);
    Task<Result> RemoveGroupMemberAsync(string? userId, string groupId, string memberId);
    Task<Result> LeaveGroupAsync(string? userId, string groupId);
    Task<Result> AddGroupAdminAsync(string? userId, string groupId, string memberId);
    Task<Result> RemoveGroupAsync(string? userId, string groupId);
}