using MessengerApp.Application.Dtos.User;
using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.UserService;

public interface IUserService
{
    public Task<Result<IEnumerable<UserPreviewDto>>> SearchUsersAsync(string? search);
    
    public Task<Result<User>> DoesUserExistAsync(string? userId);
}