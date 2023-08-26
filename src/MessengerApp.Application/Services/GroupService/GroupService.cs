using AutoMapper;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Services.UserService;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.GroupService;

public sealed class GroupService : IGroupService
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GroupService(IDbContext dbContext, IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<Result<GroupDto>> GetGroupAsync(string? userId, string groupId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<GroupPreviewDto>>> GetGroupPreviewsAsync(string? userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<string>> CreateGroupAsync(string? userId, GroupInfoDto groupInfoDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> AddGroupMemberAsync(string? userId, string groupId, string memberId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> RemoveGroupMemberAsync(string? userId, string groupId, string memberId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> LeaveGroupAsync(string? userId, string groupId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> AddGroupAdminAsync(string? userId, string groupId, string memberId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> RemoveGroupAsync(string? userId, string groupId)
    {
        throw new NotImplementedException();
    }
}