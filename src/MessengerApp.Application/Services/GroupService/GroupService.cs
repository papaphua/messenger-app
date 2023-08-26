using AutoMapper;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Services.UserService;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
using MessengerApp.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

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
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        var group = await _dbContext.Set<Group>()
            .Include(g => g.Users)
            .FirstOrDefaultAsync(group => group.Id == groupId &&
                                           group.Users.Any(u => u.Id == user.Id));
        
        if (group == null)
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var groupDto = _mapper.Map<GroupDto>(group);

        return new Result<GroupDto>
        {
            Data = groupDto
        };
    }

    public async Task<Result<IEnumerable<GroupPreviewDto>>> GetGroupPreviewsAsync(string? userId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<IEnumerable<GroupPreviewDto>>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        var groups = await _dbContext.Set<Group>()
            .Where(group => group.Users.Any(u => u.Id == user.Id))
            .ToListAsync();
        
        if (groups.Count == 0)
            return new Result<IEnumerable<GroupPreviewDto>>
            {
                Message = Results.ChatsEmpty
            };

        var groupPreviews = groups.Select(g => _mapper.Map<GroupPreviewDto>(g))
            .ToList();

        return new Result<IEnumerable<GroupPreviewDto>>
        {
            Data = groupPreviews
        };
    }

    public async Task<Result<GroupDto>> CreateGroupAsync(string? userId, GroupInfoDto groupInfoDto)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        var group = new Group();
        _mapper.Map(groupInfoDto, group);
        group.Owner = user;

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _dbContext.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();
            
            var groupUser = GroupUser.AddUserToGroup(group.Id, user.Id);

            await _dbContext.AddAsync(groupUser);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Results.ChatCreateError
            };
        }
        
        await transaction.CommitAsync();

        var newGroup = await _dbContext.Set<Group>()
            .FirstAsync(g => g.Id == group.Id);

        var newGroupDto = _mapper.Map<GroupDto>(newGroup);
        
        return new Result<GroupDto>
        {
            Data = newGroupDto
        };
    }

    public async Task<Result> AddGroupMemberAsync(string? userId, string groupId, string memberId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<string>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        var group = await _dbContext.Set<Group>()
            .Include(g => g.Users)
            .FirstOrDefaultAsync(group => group.Id == groupId);
        
        if (group == null || group.Users.All(u => u.Id != user.Id))
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        if (group.Users.Any(u => u.Id == memberId))
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatAlreadyMember
            };

        try
        {
            var userGroup = GroupUser.AddUserToGroup(group.Id, memberId);
            await _dbContext.AddAsync(userGroup);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatAddMemberError
            };
        }

        return new Result();
    }

    public async Task<Result> RemoveGroupMemberAsync(string? userId, string groupId, string memberId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<string>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        var group = await _dbContext.Set<Group>()
            .Include(g => g.Owner)
            .Include(g => g.Users)
            .Include(g => g.Admins)
            .FirstOrDefaultAsync(group => group.Id == groupId);
        
        if (group == null || group.Users.All(u => u.Id != user.Id))
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        if (group.Admins.All(u => u.Id != user.Id) || group.Owner.Id != user.Id)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNoPermission
            };
                
        if (group.Users.All(u => u.Id != memberId))
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotMember
            };

        var groupUser = await _dbContext.Set<GroupUser>()
            .FirstAsync(g => g.GroupId == groupId && g.UserId == memberId);

        try
        {
            _dbContext.Remove(groupUser);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatRemoveMemberError
            };
        }

        return new Result();
    }

    public async Task<Result> LeaveGroupAsync(string? userId, string groupId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<string>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;
        
        var group = await _dbContext.Set<Group>()
            .Include(g => g.Owner)
            .Include(g => g.Users)
            .Include(g => g.Admins)
            .FirstOrDefaultAsync(group => group.Id == groupId);
        
        if (group == null || group.Users.All(u => u.Id != user.Id))
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };
        
        if (group.Users.All(u => u.Id != user.Id))
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotMember
            };

        try
        {
            var groupUser = await _dbContext.Set<GroupUser>()
                .FirstAsync(g => g.GroupId == groupId && g.UserId == user.Id);
            
            if (group.Owner.Id == user.Id && group.Admins.Count >= 1)
            {
                var newOwner = group.Admins.First();

                var groupAdmin = await _dbContext.Set<GroupAdmin>()
                    .FirstAsync(g => g.GroupId == groupId && g.AdminId == newOwner.Id);
                
                _dbContext.Remove(groupAdmin);
                
                group.Owner = newOwner;
            }
           
            _dbContext.Remove(groupUser);

            if (group.Users.Count <= 1)
            {
                _dbContext.Remove(group);
            }
            
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatRemoveError
            };
        }

        return new Result();
    }

    public async Task<Result> AddGroupAdminAsync(string? userId, string groupId, string memberId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<string>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        var group = await _dbContext.Set<Group>()
            .Include(g => g.Owner)
            .Include(g => g.Users)
            .Include(g => g.Admins)
            .FirstOrDefaultAsync(group => group.Id == groupId);
        
        if (group == null || group.Users.All(u => u.Id != user.Id))
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        if (group.Admins.All(u => u.Id != user.Id) || group.Owner.Id != user.Id)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNoPermission
            };

        if (group.Admins.Any(u => u.Id == memberId))
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatAlreadyAdmin
            };
        
        try
        {
            var groupAdmin = GroupAdmin.AddAdminToGroup(group.Id, memberId);
            await _dbContext.AddAsync(groupAdmin);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatAddMemberError
            };
        }

        return new Result();
    }

    public async Task<Result> RemoveGroupAsync(string? userId, string groupId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<string>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        var group = await _dbContext.Set<Group>()
            .FirstOrDefaultAsync(group => group.Id == groupId);
        
        if (group == null || group.Users.All(u => u.Id != user.Id))
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        if (group.Owner.Id == user.Id)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNoPermission
            };

        try
        {
            _dbContext.Remove(group);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatRemoveError
            };
        }

        return new Result();
    }
}