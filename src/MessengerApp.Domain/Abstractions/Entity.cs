namespace MessengerApp.Domain.Abstractions;

public abstract class Entity : IEntity
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }
}