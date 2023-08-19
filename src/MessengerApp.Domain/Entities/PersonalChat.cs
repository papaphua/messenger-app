using MessengerApp.Domain.Abstractions;

namespace MessengerApp.Domain.Entities;

public sealed class PersonalChat : Chat, IEntity
{
    public Guid Id { get; set; }
}