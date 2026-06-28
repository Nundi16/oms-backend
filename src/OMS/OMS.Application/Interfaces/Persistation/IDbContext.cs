using OMS.Common.Interfaces.Entity;

namespace OMS.Application.Interfaces.Persistation
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity<Guid>;
        ValueTask<TEntity?> FindAsync<TEntity>(Guid id, CancellationToken cancellationToken = default) where TEntity : class, IEntity<Guid>;
        TEntity Update<TEntity>(TEntity entity) where TEntity : class, IEntity<Guid>;
        void SetValues<TEntity>(TEntity tracked, TEntity incoming) where TEntity : class, IEntity<Guid>;
        void Remove<TEntity>(TEntity entity) where TEntity : class, IEntity<Guid>;
    }
}
