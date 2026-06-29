using Microsoft.EntityFrameworkCore;
using OMS.Application.Interfaces.Persistation;
using OMS.Common.Interfaces.Entity;
using OMS.Infrastructure.Abstractions.Configuration;

namespace OMS.Infrastructure
{
    public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityConfiguration<>).Assembly);
        }

        public new async Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity<Guid> =>
            (await base.AddAsync(entity, cancellationToken)).Entity;

        public async ValueTask<TEntity?> FindAsync<TEntity>(Guid id, CancellationToken cancellationToken = default) where TEntity : class, IEntity<Guid> =>
            await base.FindAsync<TEntity>([id], cancellationToken);

        public new void Remove<TEntity>(TEntity entity) where TEntity : class, IEntity<Guid>
        {
            _ = base.Remove(entity);
        }

        public new TEntity Update<TEntity>(TEntity entity) where TEntity : class, IEntity<Guid> =>
            base.Update(entity).Entity;

        public void SetValues<TEntity>(TEntity tracked, TEntity incoming) where TEntity : class, IEntity<Guid> =>
            Entry(tracked).CurrentValues.SetValues(incoming);
    }
}
