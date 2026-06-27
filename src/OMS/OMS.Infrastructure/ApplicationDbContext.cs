using Microsoft.EntityFrameworkCore;
using OMS.Application.Interfaces.Persistation;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Entity;
using OMS.Infrastructure.Abstractions.Configuration;

namespace OMS.Infrastructure
{
    internal sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityConfiguration<>).Assembly);
        }

        public new async Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity<Guid> =>
            (await base.AddAsync(entity, cancellationToken)).Entity;

        public new void Remove<TEntity>(TEntity entity) where TEntity : class, IEntity<Guid>
        {
            _ = base.Remove(entity);
        }

        public new TEntity Update<TEntity>(TEntity entity) where TEntity : class, IEntity<Guid> =>
            base.Update(entity).Entity;
    }
}
