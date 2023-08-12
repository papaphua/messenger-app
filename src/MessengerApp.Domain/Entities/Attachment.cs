using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Attachment : Entity
{
    public Attachment(Guid id)
        : base(id)
    {
    }

    public required Guid MessageId { get; set; }
    
    public required Message Message { get; set; }
    
    public required byte[] Content { get; set; }
}