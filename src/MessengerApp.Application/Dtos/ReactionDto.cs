using MessengerApp.Application.Dtos.User;

namespace MessengerApp.Application.Dtos;

public sealed class ReactionDto
{
    public string Id { get; set; } = null!;

    public int ReactionNum { get; set; }

    public UserPreviewDto User { get; set; } = null!;
}