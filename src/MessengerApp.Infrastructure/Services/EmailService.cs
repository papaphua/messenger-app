using MessengerApp.Application.Abstractions.Services;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Infrastructure.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MessengerApp.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly SendGridClient _client;
    private readonly EmailOptions _options;

    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;
        _client = new SendGridClient(Environment.GetEnvironmentVariable(Variables.SendGridApiKey));
    }

    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var message = new SendGridMessage
        {
            From = new EmailAddress(_options.SenderEmail, _options.SenderName),
            Subject = emailMessage.Subject,
            PlainTextContent = emailMessage.Content
        };
        
        message.AddTo(new EmailAddress(emailMessage.Receiver));

        await _client.SendEmailAsync(message);
    }
}