using AutoMapper;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Channel;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
using MessengerApp.Domain.Enumerations;
using MessengerApp.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Services.ChannelService;

public sealed class ChannelService : IChannelService
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public ChannelService(IDbContext dbContext, IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result<ChannelDto>> GetChannelAsync(string userId, string channelId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channel = await _dbContext.Set<Channel>()
            .Include(channel => channel.Messages)
            .ThenInclude(message => message.Attachments)
            .Include(direct => direct.Messages)
            .ThenInclude(channel => channel.Reactions)
            .Include(channel => channel.Members)
            .FirstOrDefaultAsync(channel => channel.Id == channelId &&
                                            channel.Members.Any(member => member.Id == userId));

        if (channel == null)
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var messageDtos = channel.Messages
            .Select(message => _mapper.Map<MessageDto>(message))
            .OrderBy(message => message.Timestamp)
            .Reverse();
        
        var channelDto = _mapper.Map<ChannelDto>(channel);
        channelDto.Messages = messageDtos;

        return new Result<ChannelDto>
        {
            Data = channelDto
        };
    }

    public async Task<Result<IEnumerable<ChannelPreviewDto>>> GetChannelPreviewsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<IEnumerable<ChannelPreviewDto>>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channels = await _dbContext.Set<Channel>()
            .Include(channel => channel.Members)
            .Where(channel => channel.Members.Any(member => member.Id == user.Id))
            .ToListAsync();

        if (channels.Count == 0)
            return new Result<IEnumerable<ChannelPreviewDto>>
            {
                Message = Results.ChatsEmpty
            };

        var channelPreviewDtos = channels
            .Select(channel => _mapper.Map<ChannelPreviewDto>(channel))
            .ToList();

        return new Result<IEnumerable<ChannelPreviewDto>>
        {
            Data = channelPreviewDtos
        };
    }

    public async Task<Result<ChannelDto>> CreateChannelAsync(string userId, ChannelInfoDto channelInfoDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channel = new Channel();
        _mapper.Map(channelInfoDto, channel);

        var channelMember = ChannelMember.AddMemberToChannel(channel.Id, user.Id);
        channelMember.IsAdmin = true;
        channelMember.IsOwner = true;

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _dbContext.AddAsync(channel);
            await _unitOfWork.SaveChangesAsync();

            await _dbContext.AddAsync(channelMember);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.ChatCreateError
            };
        }

        await transaction.CommitAsync();

        return await GetChannelAsync(user.Id, channel.Id);
    }

    public async Task<Result<ChannelDto>> JoinChannelAsync(string userId, string channelId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channel = await _dbContext.Set<Channel>()
            .Include(channel => channel.Members)
            .FirstOrDefaultAsync(channel => channel.Id == channelId);

        if (channel == null)
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var channelMember = await _dbContext.Set<ChannelMember>()
            .FirstOrDefaultAsync(cm => cm.ChannelId == channel.Id
                                       && cm.MembersId == user.Id);

        if (channelMember != null)
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.ChatAlreadyMember
            };

        channelMember = ChannelMember.AddMemberToChannel(channel.Id, user.Id);

        try
        {
            await _dbContext.AddAsync(channelMember);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result<ChannelDto>
            {
                Succeeded = false,
                Message = Results.ChatJoinError
            };
        }

        return await GetChannelAsync(user.Id, channel.Id);
    }

    public async Task<Result> LeaveChannelAsync(string userId, string channelId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channel = await _dbContext.Set<Channel>()
            .Include(channel => channel.Members)
            .FirstOrDefaultAsync(channel => channel.Id == channelId);

        if (channel == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var channelMember = await _dbContext.Set<ChannelMember>()
            .FirstOrDefaultAsync(member => member.ChannelId == channel.Id);

        if (channelMember == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        try
        {
            if (channelMember is { IsOwner: true, IsAdmin: true })
            {
                var otherOwners = await _dbContext.Set<ChannelMember>()
                    .Where(member => member.ChannelId == channel.Id &&
                                     member.MembersId != channelMember.MembersId &&
                                     member.IsOwner)
                    .ToListAsync();

                var otherAdmins = await _dbContext.Set<ChannelMember>()
                    .Where(member => member.ChannelId == channel.Id &&
                                     member.MembersId != channelMember.MembersId &&
                                     !member.IsOwner && member.IsAdmin)
                    .ToListAsync();

                var otherMembers = await _dbContext.Set<ChannelMember>()
                    .Where(member => member.ChannelId == channel.Id &&
                                     member.MembersId != channelMember.MembersId &&
                                     !member.IsOwner && !member.IsAdmin)
                    .ToListAsync();

                if (otherOwners.Count >= 1)
                {
                    _dbContext.Remove(channelMember);
                }
                else if (otherAdmins.Count >= 1)
                {
                    var admin = otherAdmins.First();
                    admin.IsOwner = true;

                    _dbContext.Remove(channelMember);
                }
                else if (otherMembers.Count >= 1)
                {
                    var member = otherMembers.First();
                    member.IsOwner = true;
                    member.IsAdmin = true;

                    _dbContext.Remove(channelMember);
                }
                else
                {
                    _dbContext.Remove(channel);
                }
            }
            else
            {
                _dbContext.Remove(channelMember);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatLeaveError
            };
        }

        return new Result();
    }

    public async Task<Result> CreateChannelMessageAsync(string userId, string channelId,
        CreateMessageDto createMessageDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var channel = await _dbContext.Set<Channel>()
            .Include(channel => channel.Members)
            .FirstOrDefaultAsync(channel => channel.Id == channelId &&
                                            channel.Members.Any(member => member.Id == user.Id));

        if (channel == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var message = new ChannelMessage
        {
            SenderId = user.Id,
            ChatId = channel.Id
        };

        var attachments = createMessageDto.Attachments?.Select(attachment => new ChannelAttachment
        {
            MessageId = message.Id,
            ContentBytes = attachment
        });

        _mapper.Map(createMessageDto, message);

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _dbContext.Set<ChannelMessage>()
                .AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            if (attachments != null)
            {
                await _dbContext.Set<ChannelAttachment>()
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
                Message = Results.MessageSendError
            };
        }

        await transaction.CommitAsync();

        return new Result();
    }

    public async Task<Result<ChannelCommentsDto>> GetCommentsAsync(string userId, string messageId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<ChannelCommentsDto>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var message = await _dbContext.Set<ChannelMessage>()
            .Include(message => message.Comments)
            .Include(message => message.Attachments)
            .FirstOrDefaultAsync(cm => cm.Id == messageId);

        if (message == null)
            return new Result<ChannelCommentsDto>
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var channel = await _dbContext.Set<Channel>()
            .Include(channel => channel.Members)
            .FirstOrDefaultAsync(channel => channel.Id == message.ChatId &&
                                            channel.Members.Any(member => member.Id == user.Id));

        if (channel == null)
            return new Result<ChannelCommentsDto>
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var messageDto = _mapper.Map<MessageDto>(message);
        var commentDtos = message.Comments.Select(comment => _mapper.Map<CommentDto>(comment))
            .OrderBy(comment => comment.Timestamp)
            .Reverse();
        
        return new Result<ChannelCommentsDto>
        {
            Data = new ChannelCommentsDto
            {
                Message = messageDto,
                Comments = commentDtos
            }
        };
    }

    public async Task<Result> CreateCommentAsync(string userId, string messageId, CreateCommentDto createCommentDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var message = await _dbContext.Set<ChannelMessage>()
            .FirstOrDefaultAsync(cm => cm.Id == messageId);

        if (message == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var channel = await _dbContext.Set<Channel>()
            .Include(channel => channel.Members)
            .FirstOrDefaultAsync(channel => channel.Id == message.ChatId &&
                                            channel.Members.Any(member => member.Id == user.Id));

        if (channel == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        if (!channel.AllowComments)
            return new Result
            {
                Succeeded = false,
                Message = Results.CommentsNotAllowed
            };
        
        var comment = new Comment
        {
            MessageId = message.Id,
            SenderId = user.Id,
            Content = createCommentDto.Content
        };

        try
        {
            await _dbContext.Set<Comment>()
                .AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Results.MessageSendError
            };
        }

        return new Result();
    }
    
    public async Task<Result> AddReactionAsync(string userId, string messageId, Reaction reaction)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var message = await _dbContext.Set<ChannelMessage>()
            .FirstOrDefaultAsync(message => message.Id == messageId &&
                                            message.Chat.Members.Any(member => member.Id == user.Id));

        if (message == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        if (!message.Chat.AllowReactions)
            return new Result
            {
                Succeeded = false,
                Message = Results.ReactionsNotAllowed
            };
        
        var previousReaction = await _dbContext.Set<ChannelReaction>()
            .FirstOrDefaultAsync(r => r.MessageId == message.Id &&
                                      r.UserId == user.Id);

        var reactionToAdd = new ChannelReaction
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
                {
                    return new Result { Succeeded = false, Message = Results.AlreadyReacted };
                }

                _dbContext.Remove(previousReaction);
            }


            await _dbContext.Set<ChannelReaction>()
                .AddAsync(reactionToAdd);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };
        }

        return new Result();
    }
}