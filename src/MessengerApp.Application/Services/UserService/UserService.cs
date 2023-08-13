using MessengerApp.Application.Abstractions;

namespace MessengerApp.Application.Services.UserService;

public sealed class UserService : IUserService
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }
    
    
}