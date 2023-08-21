using MessengerApp.Domain.Entities;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.UserService;

public interface IUserService
{
    public Task<Result<User>> DoesUserExist(string? userId);
}