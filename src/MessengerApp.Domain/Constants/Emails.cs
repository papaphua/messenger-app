namespace MessengerApp.Domain.Constants;

public static class Emails
{
    public const string EmailConfirmationSubject = "Email adress confirmation";
    public static string EmailConfirmationMessage(string token) => $"Follow this link to confirm your email address: {token}";
}