using OMS.Application.Interfaces.Persistation;
using OMS.Common.Abstractions.Entity;

namespace OMS.Infrastructure.Persistence
{
    internal sealed class EfRepository<TEntity>(ApplicationDbContext context) : IRepository<TEntity>
        where TEntity : Entity
    {
        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
            (await context.AddAsync(entity, cancellationToken)).Entity;

        public TEntity Update(TEntity entity) => context.Update(entity).Entity;

        public TEntity Remove(TEntity entity) => context.Remove(entity).Entity;

        public IQueryable<TEntity> Query() => context.Set<TEntity>().AsQueryable();
    }
}
