namespace MessengerApp.Domain.Constants;

public static class Results
{
    // User
    public const string UserNotFound = "User not found.";

    // Profile
    public const string ProfileUpdated = "Profile updated.";
    public const string ProfilePictureUpdate = "Profile picture updated.";
    public const string PasswordChanged = "User password changed.";
    public const string EmailAlreadyConfirmed = "Email address already confirmed.";
    public const string EmailConfirmed = "Email address confirmed.";
    public const string EmailChanged = "Email address changed.";
    public const string EmailSameAsCurrect = "Email address must be different from current.";
    public const string EmailChangeError = "Could not change email, try again.";
    public static string EmailAlreadyTaken(string email) => $"Email address {email} already used.";
    
    public static string EmailChangeRequestSentTo(string email) => $"Email change confirmation sent to {email}.";
    public static string EmailConfirmationRequestSentTo(string email) => $"Email confirmation link sent to {email}.";
    

    public const string ExternalUserPasswordError =
        "Users authenticated using third party services can not change their password.";

    public const string ExternalUserEmailError =
        "Users authenticated using third party services can not change their email address.";

    // Search
    public static string NoSearchResultsFor(string? search) => $"No search results for {search}.";
}