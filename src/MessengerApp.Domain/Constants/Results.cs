namespace MessengerApp.Domain.Constants;

public static class Results
{
    public const string UserNotFound = "User not found.";

    public const string UserWithExternalLogin =
        "Users authenticated using external services can't change their email and password.";

    public const string UserProfileUpdated = "Profile information saved.";
    public const string UserEmailChanged = "Email succesfully changed.";
    public const string UserPasswordChanged = "Password succesfully changed.";
    public const string ConfirmationMessageSent = "Confirmation message was sent to your email.";
}