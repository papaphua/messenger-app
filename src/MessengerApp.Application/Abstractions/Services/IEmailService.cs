namespace MessengerApp.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendEmailAsync(string receiver, string subject, string content);
}