using System.ComponentModel;

namespace MessengerApp.Application.Dtos.User;

public sealed class UserPreviewDto
{
    public Guid Id { get; set; }

    [DisplayName("Username")] public string UserName { get; set; } = null!;

    [DisplayName("First name")] public string? FirstName { get; set; }

    [DisplayName("Last name")] public string? LastName { get; set; }

    public byte[]? ProfilePicture { get; set; }
}