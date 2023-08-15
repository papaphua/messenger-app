using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class Attachment : IEntity
{
    public required Guid MessageId { get; set; }

    public required Message Message { get; set; }

    public required byte[] Content { get; set; }
    public Guid Id { get; set; }
}