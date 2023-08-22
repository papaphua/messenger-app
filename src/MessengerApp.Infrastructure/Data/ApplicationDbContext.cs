using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Domain.Abstractions;
using MessengerApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MessengerApp.Infrastructure.Data;

public sealed class ApplicationDbContext : IdentityDbContext<User>, IDbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : class, IEntity
    {
        return ((DbContext)this).Set<TEntity>();
    }

    public async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity
    {
        await Set<TEntity>().AddAsync(entity);
    }

    public new void Update<TEntity>(TEntity entity)
        where TEntity : class, IEntity
    {
        Set<TEntity>().Update(entity);
    }

    public new void Remove<TEntity>(TEntity entity)
        where TEntity : class, IEntity
    {
        Set<TEntity>().Remove(entity);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}