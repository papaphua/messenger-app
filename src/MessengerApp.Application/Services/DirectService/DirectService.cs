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

    public async Task<Result<IEnumerable<DirectPreviewDto>>> GetDirectPreviewsAsync(string? userId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result<IEnumerable<DirectPreviewDto>>
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

        var directs = await _dbContext.Set<Direct>()
            .Include(u => u.Users)
            .Where(direct => direct.Users
                .Any(u => u.Id == user.Id))
            .ToListAsync();

        if (directs.Count == 0)
            return new Result<IEnumerable<DirectPreviewDto>>
            {
                Message = Results.ChatsEmpty
            };

        var directPreviews = directs.Select(direct =>
        {
            var conversator = direct.Users.FirstOrDefault(u => u.Id != user.Id)!;

            var fullName = string.Join(' ', conversator.FirstName, conversator.LastName);

            var title = string.IsNullOrWhiteSpace(fullName) ? conversator.UserName! : fullName;

            return new DirectPreviewDto
            {
                Id = direct.Id,
                Title = title,
                ProfilePictureBytes = conversator.ProfilePictureBytes
            };
        }).ToList();

        return new Result<IEnumerable<DirectPreviewDto>>
        {
            Data = directPreviews
        };
    }

    public async Task<Result> CreateDirectAsync(string? userId, string conversatorId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);
        var doesConversatorExistResult = await _userService.DoesUserExistAsync(conversatorId);

        if (!doesUserExistResult.Succeeded || !doesConversatorExistResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = doesConversatorExistResult.Message ?? doesConversatorExistResult.Message
            };

        var user = doesUserExistResult.Data!;
        var conversator = doesConversatorExistResult.Data!;

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

            var directUser = DirectUser.AddUserToDirect(direct.Id, user.Id);
            var directConversator = DirectUser.AddUserToDirect(direct.Id, conversator.Id);

            await _dbContext.AddRangeAsync(directUser, directConversator);
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

    public async Task<Result> RemoveDirectAsync(string? userId, string directId)
    {
        var doesUserExistResult = await _userService.DoesUserExistAsync(userId);

        if (!doesUserExistResult.Succeeded)
            return new Result
            {
                Succeeded = false,
                Message = doesUserExistResult.Message
            };

        var user = doesUserExistResult.Data!;

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