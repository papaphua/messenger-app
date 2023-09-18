namespace MessengerApp.Domain.Abstractions.Chat;

public interface IAdministratedChat
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public byte[]? ChatPictureBytes { get; set; }

    public bool IsPrivate { get; set; }
    
    public bool AllowReactions { get; set; }
    
    public bool AllowComments { get; set; }
}