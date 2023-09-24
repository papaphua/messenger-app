using System.ComponentModel;

namespace MessengerApp.Application.Dtos.User;

public sealed class UserPreviewDto
{
    public string Id { get; set; } = null!;

    [DisplayName("Username")] public string UserName { get; set; } = null!;

    [DisplayName("FirstName")] public string? FirstName { get; set; }

    [DisplayName("LastName")] public string? LastName { get; set; }

    public byte[]? ProfilePictureBytes { get; set; }
}