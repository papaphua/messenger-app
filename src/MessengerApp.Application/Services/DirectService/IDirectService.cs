using MessengerApp.Application.Dtos;
using MessengerApp.Application.Dtos.Direct;
using MessengerApp.Domain.Enumerations;
using MessengerApp.Domain.Primitives;

namespace MessengerApp.Application.Services.DirectService;

public interface IDirectService
{
    Task<Result<DirectDto>> GetDirectAsync(string userId, string directId);
    Task<Result<IEnumerable<DirectPreviewDto>>> GetDirectPreviewsAsync(string userId);
    Task<Result<DirectDto>> CreateDirectAsync(string userId, string conversatorId);
    Task<Result> RemoveDirectAsync(string userId, string directId);
    Task<Result> CreateDirectMessageAsync(string userId, string directId, CreateMessageDto createMessageDto);
    Task<Result> AddReactionAsync(string userId, string messageId, Reaction reaction);
}