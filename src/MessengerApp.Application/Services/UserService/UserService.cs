using AutoMapper;
using MessengerApp.Application.Dtos;
using MessengerApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

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


    public async Task<UserDto> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        // TODO not authorized exception
        if (user == null) throw new Exception();

        return _mapper.Map<User, UserDto>(user);
    }

    public async Task UpdateUserProfileAsync(string userId, UserProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        // TODO not authorized exception
        if (user == null) throw new Exception();

        _mapper.Map(dto, user);

        await _userManager.UpdateAsync(user);
    }

    public Task ChangeEmailAsync(string userId, UserEmailDto emailDto)
    {
        throw new NotImplementedException();
    }

    public Task ChangePasswordAsync(string userId, UserPasswordDto passwordDto)
    {
        throw new NotImplementedException();
    }
}