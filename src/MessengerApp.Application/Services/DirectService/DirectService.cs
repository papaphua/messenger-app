using AutoMapper;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Direct;
using MessengerApp.Application.Helpers;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
using MessengerApp.Domain.Enumerations;
using MessengerApp.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Services.DirectService;

public sealed class DirectService : IDirectService
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public DirectService(IDbContext dbContext, IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result<DirectDto>> GetDirectAsync(string userId, string directId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<DirectDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var direct = await _dbContext.Set<Direct>()
            .Include(direct => direct.Messages)
            .ThenInclude(message => message.Attachments)
            .Include(direct => direct.Messages)
            .ThenInclude(message => message.Reactions)
            .Include(direct => direct.Members)
            .FirstOrDefaultAsync(direct => direct.Id == directId &&
                                           direct.Members.Any(member => member.Id == user.Id));

        if (direct == null)
            return new Result<DirectDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        var conversator = direct.Members.First(member => member.Id != user.Id);

        var messageDtos = direct.Messages
            .Select(message => _mapper.Map<MessageDto>(message))
            .OrderBy(message => message.Timestamp)
            .Reverse()
            .ToList();

        var directDto = new DirectDto();
        _mapper.Map(conversator, directDto);

        directDto.Id = direct.Id;

        directDto.Messages = messageDtos;

        return new Result<DirectDto>
        {
            Data = directDto
        };
    }

    public async Task<Result<IReadOnlyList<DirectPreviewDto>>> GetDirectPreviewsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<IReadOnlyList<DirectPreviewDto>>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var directs = await _dbContext.Set<Direct>()
            .Include(direct => direct.Members)
            .Where(direct => direct.Members.Any(member => member.Id == user.Id))
            .ToListAsync();

        if (directs.Count == 0)
            return new Result<IReadOnlyList<DirectPreviewDto>>
            {
                Message = Localizer.GetLocalizedResult(Results.ChatsEmpty)
            };

        var directPreviewDtos = directs.Select(direct =>
        {
            var conversator = direct.Members.First(members => members.Id != user.Id);
            var conversatorFullName = string.Join(' ', conversator.FirstName, conversator.LastName);
            var directTitle = string.IsNullOrWhiteSpace(conversatorFullName)
                ? conversator.UserName!
                : conversatorFullName;

            return new DirectPreviewDto
            {
                Id = direct.Id,
                Title = directTitle,
                ProfilePictureBytes = conversator.ProfilePictureBytes
            };
        }).ToList();

        return new Result<IReadOnlyList<DirectPreviewDto>>
        {
            Data = directPreviewDtos
        };
    }

    public async Task<Result<DirectDto>> CreateDirectAsync(string userId, string conversatorId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var conversator = await _userManager.FindByIdAsync(conversatorId);

        if (user == null || conversator == null)
            return new Result<DirectDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var direct = await _dbContext.Set<Direct>()
            .Include(direct => direct.Members)
            .Where(direct => direct.Members.Any(member => member.Id == userId) &&
                             direct.Members.Any(member => member.Id == conversatorId))
            .FirstOrDefaultAsync();

        if (direct != null)
            return await GetDirectAsync(user.Id, direct.Id);

        direct = new Direct();

        var directUser = DirectMember.AddMemberToDirect(direct.Id, user.Id);
        var directConversator = DirectMember.AddMemberToDirect(direct.Id, conversator.Id);

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _dbContext.AddAsync(direct);
            await _unitOfWork.SaveChangesAsync();

            await _dbContext.AddRangeAsync(directUser, directConversator);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            return new Result<DirectDto>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatCreateError)
            };
        }

        await transaction.CommitAsync();

        return await GetDirectAsync(user.Id, direct.Id);
    }

    public async Task<Result> RemoveDirectAsync(string userId, string directId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var direct = await _dbContext.Set<Direct>()
            .Include(direct => direct.Members)
            .FirstOrDefaultAsync(direct => direct.Id == directId &&
                                           direct.Members.Any(member => member.Id == user.Id));

        if (direct == null)
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        try
        {
            _dbContext.Remove(direct);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatRemoveError)
            };
        }

        return new Result();
    }

    public async Task<Result> CreateDirectMessageAsync(string userId, string directId,
        CreateMessageDto createMessageDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var direct = await _dbContext.Set<Direct>()
            .Include(direct => direct.Members)
            .FirstOrDefaultAsync(direct => direct.Id == directId &&
                                           direct.Members.Any(member => member.Id == user.Id));

        if (direct == null)
            return new Result
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        var message = new DirectMessage
        {
            SenderId = user.Id,
            ChatId = direct.Id
        };

        var attachments = createMessageDto.Attachments?.Select(attachment => new DirectAttachment
        {
            MessageId = message.Id,
            ContentBytes = attachment
        });

        _mapper.Map(createMessageDto, message);

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _dbContext.Set<DirectMessage>()
                .AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            if (attachments != null)
            {
                await _dbContext.Set<DirectAttachment>()
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

    public async Task<Result<string>> CreateDirectReactionAsync(string userId, string messageId, Reaction reaction)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<string>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.UserNotFound)
            };

        var message = await _dbContext.Set<DirectMessage>()
            .FirstOrDefaultAsync(message => message.Id == messageId &&
                                            message.Chat.Members.Any(member => member.Id == user.Id));

        if (message == null)
            return new Result<string>
            {
                Succeeded = false,
                Message = Localizer.GetLocalizedResult(Results.ChatNotFound)
            };

        var previousReaction = await _dbContext.Set<DirectReaction>()
            .FirstOrDefaultAsync(r => r.MessageId == message.Id &&
                                      r.UserId == user.Id);

        var reactionToAdd = new DirectReaction
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


            await _dbContext.Set<DirectReaction>()
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
}