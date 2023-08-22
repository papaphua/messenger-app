using AutoMapper;
using MessengerApp.Application.Dtos.User;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Services.UserService;

public sealed class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<UserPreviewDto>>> SearchUsersAsync(string? search)
    {
        if (string.IsNullOrEmpty(search))
            return new Result<IEnumerable<UserPreviewDto>>
            {
                Message = Results.NoSearchResults
            };
        
        var users = await _userManager.Users
            .Where(user => EF.Functions.Like(user.UserName, $"%{search}%"))
            .ToListAsync();

        if (users.Count == 0)
            return new Result<IEnumerable<UserPreviewDto>>
            {
                Message = Results.NoSearchResults
            };

        var userPreviews = users.Select(user => _mapper.Map<UserPreviewDto>(user));

        return new Result<IEnumerable<UserPreviewDto>>
        {
            Data = userPreviews
        };
    }

    public async Task<Result<User>> DoesUserExistAsync(string? userId)
    {
        if (userId == null)
            return new Result<User>
            {
                Succeeded = false,
                Message = Results.UserNotAuthenticated
            };

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return new Result<User>
            {
                Succeeded = false,
                Message = Results.UserNotFound
            };

        return new Result<User>
        {
            Data = user
        };
    }
}