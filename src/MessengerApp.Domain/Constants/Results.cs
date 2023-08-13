namespace MessengerApp.Domain.Constants;

public static class Results
{
    public const string UserNotFound = "User not found.";

    public const string UserWithExternalLogin =
        "Users authenticated using external services can't change their email and password.";

    public const string UserProfileUpdated = "Profile changes saved.";
    public const string UserEmailUpdated = "Email succesfully changed.";
    public const string UserPasswordUpdated = "Password succesfully changed.";
}