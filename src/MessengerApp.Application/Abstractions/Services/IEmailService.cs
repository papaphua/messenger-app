namespace MessengerApp.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendEmailAsync(string receiverEmail, string subject, string content);
}