namespace MessengerApp.Domain.Constants;

public static class Emails
{
    public const string EmailConfirmationSubject = "Email address confirmation";
    public const string EmailChangeSubject = "Email change confirmation";

    public static string EmailConfirmationMessage(string link)
    {
        return $"Follow this link to confirm your email address: {link}";
    }

    public static string EmailChangeMessage(string link)
    {
        return $"Follow this link to change your email address: {link}";
    }
}