using MessengerApp.Application.Abstractions.Services;
using MessengerApp.Domain.Constants;
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
        _client = new SendGridClient(Environment.GetEnvironmentVariable(Envs.SendGridApiKey));
    }

    public async Task SendEmailAsync(string receiverEmail, string subject, string content)
    {
        var message = new SendGridMessage
        {
            From = new EmailAddress(_options.SenderEmail, _options.SenderName),
            Subject = subject,
            PlainTextContent = content
        };

        message.AddTo(new EmailAddress(receiverEmail));

        await _client.SendEmailAsync(message);
    }
}