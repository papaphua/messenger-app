using MessengerApp.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Abstractions;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class, IEntity;
    
    Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
        where TEntity : class, IEntity;
    
    Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity;

    void Update<TEntity>(TEntity entity)
        where TEntity : class, IEntity;

    void Remove<TEntity>(TEntity entity)
        where TEntity : class, IEntity;
}