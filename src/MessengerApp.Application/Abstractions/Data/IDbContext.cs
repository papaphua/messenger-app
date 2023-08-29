using MessengerApp.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Abstractions.Data;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class, IEntity;

    Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity;

    Task AddRangeAsync<TEntity>(params TEntity[] entities)
        where TEntity : class, IEntity;

    void Update<TEntity>(TEntity entity)
        where TEntity : class, IEntity;

    void Remove<TEntity>(TEntity entity)
        where TEntity : class, IEntity;
}