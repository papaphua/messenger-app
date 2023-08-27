namespace MessengerApp.Domain.Abstractions.Chat;

public interface IAdministratedChat
{
    public string Title { get; set; }
    
    public string? Description { get; set; }

    public byte[]? ChatPictureBytes { get; set; }
}