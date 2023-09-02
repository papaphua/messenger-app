using MessengerApp.Application.Dtos.Profile;
using MessengerApp.Domain.Entities;

namespace MessengerApp.Application.Dtos.Direct;

public sealed class DirectDto
{
    public string Id { get; set; } = null!;

    public ProfileInfoDto ProfileInfoDto { get; set; } = null!;

    public byte[]? ProfilePictureBytes { get; set; }

    public IEnumerable<DirectMessage> Messages { get; set; } = null!;
}