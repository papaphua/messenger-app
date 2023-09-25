using AutoMapper;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Group;
using MessengerApp.Application.Helpers;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
using MessengerApp.Domain.Enumerations;
using MessengerApp.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Services.GroupService;

public sealed class GroupService : IGroupService
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public GroupService(IDbContext dbContext, IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result<GroupDto>> GetGroupAsync(string userId, string groupId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var group = await _dbContext.Set<Group>()
            .Include(group => group.Messages)
            .ThenInclude(message => message.Attachments)
            .Include(group => group.Messages)
            .ThenInclude(message => message.Reactions)
            .Include(group => group.Members)
            .FirstOrDefaultAsync(group => group.Id == groupId &&
                                          group.Members.Any(member => member.Id == userId));

        if (group == null)
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        var messageDtos = group.Messages
            .Select(message => _mapper.Map<MessageDto>(message))
            .OrderBy(message => message.Timestamp)
            .Reverse()
            .ToList();

        var groupDto = _mapper.Map<GroupDto>(group);
        groupDto.Messages = messageDtos;

        return new Result<GroupDto>
        {
            Data = groupDto
        };
    }

    public async Task<Result<IReadOnlyList<GroupPreviewDto>>> GetGroupPreviewsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<IReadOnlyList<GroupPreviewDto>>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var groups = await _dbContext.Set<Group>()
            .Include(group => group.Members)
            .Where(group => group.Members.Any(member => member.Id == user.Id))
            .ToListAsync();

        if (groups.Count == 0)
            return new Result<IReadOnlyList<GroupPreviewDto>>
            {
                Message = Localizer.GetLocalizedResult(Results.ChatsEmpty)
            };

        var groupPreviewDtos = groups
            .Select(group => _mapper.Map<GroupPreviewDto>(group))
            .ToList();

        return new Result<IReadOnlyList<GroupPreviewDto>>
        {
            Data = groupPreviewDtos
        };
    }

    public async Task<Result<GroupDto>> CreateGroupAsync(string userId, GroupInfoDto groupInfoDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var group = new Group();
        _mapper.Map(groupInfoDto, group);

        var groupMember = GroupMember.AddMemberToGroup(group.Id, user.Id);
        groupMember.IsAdmin = true;
        groupMember.IsOwner = true;

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _dbContext.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            await _dbContext.AddAsync(groupMember);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatCreateError)
            };
        }

        await transaction.CommitAsync();

        return await GetGroupAsync(user.Id, group.Id);
    }

    public async Task<Result<GroupDto>> JoinGroupAsync(string userId, string groupId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var group = await _dbContext.Set<Group>()
            .Include(group => group.Members)
            .FirstOrDefaultAsync(group => group.Id == groupId);

        if (group == null)
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        var groupMember = await _dbContext.Set<GroupMember>()
            .FirstOrDefaultAsync(gm => gm.GroupId == group.Id
                                       && gm.MembersId == user.Id);

        if (groupMember != null)
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatAlreadyMember)
            };

        groupMember = GroupMember.AddMemberToGroup(group.Id, user.Id);

        try
        {
            await _dbContext.AddAsync(groupMember);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result<GroupDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatJoinError)
            };
        }

        return await GetGroupAsync(user.Id, group.Id);
    }

    public async Task<Result> LeaveGroupAsync(string userId, string groupId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var group = await _dbContext.Set<Group>()
            .Include(group => group.Members)
            .FirstOrDefaultAsync(group => group.Id == groupId);

        if (group == null)
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        var groupMember = await _dbContext.Set<GroupMember>()
            .FirstOrDefaultAsync(member => member.GroupId == group.Id);

        if (groupMember == null)
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        try
        {
            if (groupMember is { IsOwner: true, IsAdmin: true })
            {
                var otherOwners = await _dbContext.Set<GroupMember>()
                    .Where(member => member.GroupId == group.Id &&
                                     member.MembersId != groupMember.MembersId &&
                                     member.IsOwner)
                    .ToListAsync();

                var otherAdmins = await _dbContext.Set<GroupMember>()
                    .Where(member => member.GroupId == group.Id &&
                                     member.MembersId != groupMember.MembersId &&
                                     !member.IsOwner && member.IsAdmin)
                    .ToListAsync();

                var otherMembers = await _dbContext.Set<GroupMember>()
                    .Where(member => member.GroupId == group.Id &&
                                     member.MembersId != groupMember.MembersId &&
                                     !member.IsOwner && !member.IsAdmin)
                    .ToListAsync();

                if (otherOwners.Count >= 1)
                {
                    _dbContext.Remove(groupMember);
                }
                else if (otherAdmins.Count >= 1)
                {
                    var admin = otherAdmins.First();
                    admin.IsOwner = true;

                    _dbContext.Remove(groupMember);
                }
                else if (otherMembers.Count >= 1)
                {
                    var member = otherMembers.First();
                    member.IsOwner = true;
                    member.IsAdmin = true;

                    _dbContext.Remove(groupMember);
                }
                else
                {
                    _dbContext.Remove(group);
                }
            }
            else
            {
                _dbContext.Remove(groupMember);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatLeaveError)
            };
        }

        return new Result();
    }

    public async Task<Result> CreateGroupMessageAsync(string userId, string groupId, CreateMessageDto createMessageDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var group = await _dbContext.Set<Group>()
            .Include(group => group.Members)
            .FirstOrDefaultAsync(group => group.Id == groupId &&
                                          group.Members.Any(member => member.Id == user.Id));

        if (group == null)
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        var message = new GroupMessage
        {
            SenderId = user.Id,
            ChatId = group.Id
        };

        var attachments = createMessageDto.Attachments?.Select(attachment => new GroupAttachment
        {
            MessageId = message.Id,
            ContentBytes = attachment
        });

        _mapper.Map(createMessageDto, message);

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _dbContext.Set<GroupMessage>()
                .AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            if (attachments != null)
            {
                await _dbContext.Set<GroupAttachment>()
                    .AddRangeAsync(attachments);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.MessageSendError)
            };
        }

        await transaction.CommitAsync();

        return new Result();
    }

    public async Task<Result<string>> CreateGroupReactionAsync(string userId, string messageId, Reaction reaction)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<string>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var message = await _dbContext.Set<GroupMessage>()
            .Include(message => message.Chat)
            .FirstOrDefaultAsync(message => message.Id == messageId &&
                                            message.Chat.Members.Any(member => member.Id == user.Id));

        if (message == null)
            return new Result<string>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        if (!message.Chat.AllowReactions)
            return new Result<string>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ReactionsNotAllowed)
            };

        var previousReaction = await _dbContext.Set<GroupReaction>()
            .FirstOrDefaultAsync(r => r.MessageId == message.Id &&
                                      r.UserId == user.Id);

        var reactionToAdd = new GroupReaction
        {
            UserId = user.Id,
            MessageId = message.Id,
            ReactionNum = (int)reaction
        };

        try
        {
            if (previousReaction != null)
            {
                if (previousReaction.ReactionNum == reactionToAdd.ReactionNum)
                    return new Result<string>
                        { Succeeded = false, Message = Localizer.GetLocalizedResult(Results.AlreadyReacted) };

                _dbContext.Remove(previousReaction);
            }


            await _dbContext.Set<GroupReaction>()
                .AddAsync(reactionToAdd);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result<string>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };
        }

        return new Result<string> { Data = message.ChatId };
    }

    public async Task<Result<GroupOptionsDto>> GetGroupOptionsAsync(string userId, string groupId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<GroupOptionsDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };
        
        var group = await _dbContext.Set<Group>()
            .FirstOrDefaultAsync(group => group.Id == groupId &&
                                          group.Members.Any(member => member.Id == user.Id));

        if (group == null)
            return new Result<GroupOptionsDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        var groupMember = await _dbContext.Set<GroupMember>()
            .FirstAsync(member => member.MembersId == user.Id &&
                                           member.GroupId == group.Id);

        //TODO add localization
        if (!groupMember.IsAdmin || !groupMember.IsOwner)
            return new Result<GroupOptionsDto>
            {
                Succeeded = false,
                Message = "NotPermission"
            };

        var options = _mapper.Map<GroupOptionsDto>(group);

        return new Result<GroupOptionsDto>
        {
            Data = options
        };
    }

    public async Task<Result> UpdateGroupOptionsAsync(string userId, string groupId, GroupOptionsDto groupOptionsDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<GroupOptionsDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };
        
        var group = await _dbContext.Set<Group>()
            .FirstOrDefaultAsync(group => group.Id == groupId &&
                                          group.Members.Any(member => member.Id == user.Id));

        if (group == null)
            return new Result<GroupOptionsDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        var groupMember = await _dbContext.Set<GroupMember>()
            .FirstAsync(member => member.MembersId == user.Id &&
                                  member.GroupId == group.Id);

        if (!groupMember.IsAdmin || !groupMember.IsOwner)
            return new Result
            {
                Succeeded = false,
                Message = "NoPermission"
            };
        
        if (groupMember.IsAdmin)
        {
            group.Title = groupOptionsDto.Title;
            group.Description = groupOptionsDto.Description;
            group.ChatPictureBytes = groupOptionsDto.ChatPictureBytes;
            
            try
            {
                _dbContext.Update(group);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new Result
                {
                    Succeeded = false,
                    Message = "ChatUpdateError"
                };
            }

            if (group.IsPrivate != groupOptionsDto.IsPrivate || group.AllowReactions != groupOptionsDto.AllowReactions)
                return new Result
                {
                    Succeeded = false,
                    Message = "OnlyChatInfoSaved"
                };

            return new Result
            {
                Message = "ChatInfoSaved"
            };
        }

        var updatedGroup = _mapper.Map(groupOptionsDto, group);

        try
        {
            _dbContext.Update(updatedGroup);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = "ChatUpdateError"
            };
        }

        return new Result
        {
            Message = "ChatInfoSaved"
        };
    }
}