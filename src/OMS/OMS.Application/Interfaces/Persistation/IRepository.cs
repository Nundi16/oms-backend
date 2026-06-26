using System.Linq;
using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Interfaces.Persistation
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        TEntity Update(TEntity entity);
        TEntity Remove(TEntity entity);
        IQueryable<TEntity> Query();
    }
}
