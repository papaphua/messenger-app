using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Dtos.Direct;
using MessengerApp.Application.Services.UserService;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Entities.Joints;
using MessengerApp.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Services.DirectService;

public sealed class DirectService : IDirectService
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public DirectService(IDbContext dbContext, IUnitOfWork unitOfWork, IUserService userService)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<Result<IEnumerable<DirectPreviewDto>>> GetDirectPreviews(string? userId)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result<IEnumerable<DirectPreviewDto>>
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        var directPreviews = await _dbContext.Set<Direct>()
            .Where(direct => direct.Users
                .Any(u => u.Id == user.Id))
            .Include(direct => direct.Users
                .Any(u => u.Id != user.Id))
            .ToListAsync();

        if (directPreviews.Count == 0)
            return new Result<IEnumerable<DirectPreviewDto>>
            {
                Message = Results.ChatsEmpty
            };

        // TODO create map
        var dtos = directPreviews.Select(direct =>
        {
            var conversator = direct.Users.FirstOrDefault()!;

            var fullName = string.Join(' ', conversator.FirstName, conversator.LastName);

            var title = string.IsNullOrWhiteSpace(fullName) ? conversator.UserName! : fullName;

            return new DirectPreviewDto
            {
                Id = direct.Id,
                Title = title
            };
        }).ToList();

        return new Result<IEnumerable<DirectPreviewDto>>
        {
            Data = dtos
        };
    }

    public async Task<Result> AddDirect(string? userId, string conversatorId)
    {
        var userResult = await _userService.DoesUserExist(userId);
        var conversatorResult = await _userService.DoesUserExist(conversatorId);

        if (!userResult.Succeeded || !conversatorResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = userResult.Message ?? conversatorResult.Message
            };

        var user = userResult.Data!;
        var conversator = conversatorResult.Data!;

        var direct = await _dbContext.Set<Direct>()
            .Where(direct => direct.Users.Any(u => u.Id == user.Id) &&
                             direct.Users.Any(u => u.Id == conversator.Id))
            .FirstOrDefaultAsync();

        if (direct != null)
        {
            return new Result();
        }

        direct = new Direct();
        
        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _dbContext.AddAsync(direct);
            await _unitOfWork.SaveChangesAsync();

            await _dbContext.Set<DirectUser>()
                .AddAsync(new DirectUser { DirectId = direct.Id, UserId = user.Id });
            await _dbContext.Set<DirectUser>()
                .AddAsync(new DirectUser { DirectId = direct.Id, UserId = conversator.Id });
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            return new Result
            {
                Succeeded = false,
                Message = Results.ChatCreateError
            };
        }

        await transaction.CommitAsync();

        return new Result();
    }

    public async Task<Result> RemoveDirect(string? userId, Guid directId)
    {
        var userResult = await _userService.DoesUserExist(userId);

        if (!userResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = userResult.Message
            };

        var user = userResult.Data!;

        var direct = await _dbContext.Set<Direct>()
            .Include(direct => direct.Users)
            .FirstOrDefaultAsync(direct => direct.Id == directId);

        if (direct == null)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatRemoveError
            };

        var isMember = direct.Users.Any(u => u.Id == user.Id);

        if (!isMember)
            return new Result
            {
                Succeeded = false,
                Message = Results.ChatRemoveError
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