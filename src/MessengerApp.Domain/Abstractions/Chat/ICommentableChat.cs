namespace MessengerApp.Domain.Abstractions.Chat;

public interface ICommentableChat
{
    public bool AllowComments { get; set; }
}