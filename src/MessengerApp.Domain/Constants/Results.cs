namespace MessengerApp.Domain.Constants;

public static class Results
{
    public const string UserNotFound = "User not found.";
    public const string UserProfileUpdated = "Profile information  updated.";
    public const string UserPasswordChanged = "Password changed.";
    public const string UserEmailConfirmed = "Email confirmed.";
    public const string UserEmailChanged = "Email changed.";
    public const string ProfilePictureUpdated = "Profile picture updated.";
    public const string RequestedEmailNotFound = "Something went wrong. Try to change email again.";
    public const string RequestedEmailSameAsCurrent = "You have to provide email different from current one.";
    public const string EmailConfirmationRequested = "Confirmation message sent to your email.";
    public const string EmailConfirmationNotAuthenticated = "You must be logged in to confirm your email.";
    public const string EmailChangeRequested = "Confirmation message sent to your new email.";
    public const string EmailChangeNotAuthenticated = "You must be logged in to change your email.";
    public const string EmailAlreadyConfirmed = "Email already confirmed";
    public const string EmailAlreadyTaken = "Email address is already taken by another user.";

    public const string ExternalUser =
        "Users authenticated using external services can't change their email and password.";
}