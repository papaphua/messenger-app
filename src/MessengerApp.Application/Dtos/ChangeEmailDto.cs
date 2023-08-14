namespace MessengerApp.Application.Dtos;

public sealed class ChangeEmailDto
{
    public string NewEmail { get; set; }

    public string Token { get; set; }
}