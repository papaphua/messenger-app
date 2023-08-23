namespace MessengerApp.Domain.Constants;

public static class Emails
{
    public const string ConfirmationSubject = "Email address confirmation";
    public const string ChangeSubject = "Email change confirmation";

    public static string GetConfirmationMessage(string link)
    {
        return $"Follow this link to confirm your email address: {link}";
    }

    public static string GetChangeMessage(string link)
    {
        return $"Follow this link to change your email address: {link}";
    }
}