using MessengerApp.Domain.Entities;

namespace MessengerApp.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}