using AutoMapper;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Dtos.Direct;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
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
                Message = Results.UserNotFound
            };

        var direct = await _dbContext.Set<Direct>()
            .FirstOrDefaultAsync(direct => direct.Id == directId &&
                                           direct.Members.Any(member => member.Id == user.Id));

        if (direct == null)
            return new Result<DirectDto>
            {
                Succeeded = false,
                Message = Results.ChatNotFound
            };

        var conversator = direct.Members.First(member => member.Id != user.Id);

        var directDto = new DirectDto { Id = direct.Id };
        _mapper.Map(conversator, directDto);

        return new Result<DirectDto>
        {
            Data = directDto
        };
    }

    public async Task<Result<IEnumerable<DirectPreviewDto>>> GetDirectPreviewsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<IEnumerable<DirectPreviewDto>>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        var directs = await _dbContext.Set<Direct>()
            .Where(direct => direct.Members.Any(member => member.Id == user.Id))
            .ToListAsync();

        if (directs.Count == 0)
            return new Result<IEnumerable<DirectPreviewDto>>
            {
                Message = Results.ChatsEmpty
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

        return new Result<IEnumerable<DirectPreviewDto>>
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
                Message = Results.UserNotFound
            };

        var direct = await _dbContext.Set<Direct>()
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
                Message = Results.ChatCreateError
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
                Message = Results.UserNotFound
            };

        var direct = await _dbContext.Set<Direct>()
            .FirstOrDefaultAsync(direct => direct.Id == directId &&
                                           direct.Members.Any(member => member.Id == user.Id));

        if (direct == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatNotFound
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
                Message = Results.ChatRemoveError
            };
        }

        return new Result();
    }
}