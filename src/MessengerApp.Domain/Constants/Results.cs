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
    public const string EmailSameAsCurrent = "Email address must be different from current.";
    public const string EmailChangeError = "Could not change email, try again.";

    public const string ExternalUserPasswordError =
        "Users authenticated using third party services can not change their password.";

    public const string ExternalUserEmailError =
        "Users authenticated using third party services can not change their email address.";

    //Message
    public const string MessageSendError = "Could not send message.";

    // Chat
    public const string ChatNotFound = "Chat not found.";
    public const string ChatsEmpty = "Chats empty.";
    public const string ChatCreateError = "Could not create chat.";
    public const string ChatRemoveError = "Could not remove chat.";
    public const string ChatLeaveError = "Could not leave chat.";
    public const string ChatJoinError = "Could not join chat.";
    public const string ChatAlreadyMember = "Already member.";

    // Reactions
    public const string AlreadyReacted = "Message is already reacted.";
    public const string ReactionsNotAllowed = "Reactions are disabled in this chat.";
    public const string CommentsNotAllowed = "Comments are disabled in this chat.";

    public static string EmailAlreadyTaken(string email)
    {
        return $"Email address {email} already used.";
    }

    public static string EmailChangeRequestSentTo(string email)
    {
        return $"Email change confirmation sent to {email}.";
    }

    public static string EmailConfirmationRequestSentTo(string email)
    {
        return $"Email confirmation link sent to {email}.";
    }

    // Search
    public static string NoSearchResultsFor(string? search)
    {
        return $"No search results for {search}.";
    }
}