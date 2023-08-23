using System.ComponentModel;

namespace MessengerApp.Application.Dtos.User;

public sealed class UserPreviewDto
{
    public string Id { get; set; } = null!;

    [DisplayName("Username")] public string UserName { get; set; } = null!;

    [DisplayName("First name")] public string? FirstName { get; set; }

    [DisplayName("Last name")] public string? LastName { get; set; }

    public byte[]? ProfilePictureBytes { get; set; }
}