using MessengerApp.Application.Dtos.User;

namespace MessengerApp.Application.Dtos;

public sealed class AttachmentDto
{

    public string Id { get; set; } = null!;
    
    public byte[] ContentBytes { get; set; } = null!;
}