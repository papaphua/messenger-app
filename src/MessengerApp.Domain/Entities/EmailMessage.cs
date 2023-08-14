namespace MessengerApp.Domain.Entities;

public sealed class EmailMessage
{
    private EmailMessage(string receiver, string subject, string content)
    {
        Receiver = receiver;
        Subject = subject;
        Content = content;
    }

    public string Receiver { get; }

    public string Subject { get; }

    public string Content { get; }

    public static EmailMessage Create(string receiver, string subject, string content)
    {
        return new EmailMessage(receiver, subject, content);
    }
}