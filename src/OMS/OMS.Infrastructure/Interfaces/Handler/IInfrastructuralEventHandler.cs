using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMS.Common.Interfaces.Communication;
using OMS.Domain;

namespace OMS.Infrastructure.Interfaces.Handler
{
    internal interface IAsyncInfrastructuralEventHandler<TEntity> : IHandler where TEntity : Entity
    {
        Task<EntityEntry<TEntity>> HandleAsync(TEntity entity, CancellationToken cancellationToken = default);
    }

    internal interface IInfrastructuralEventHandler<TEntity> : IHandler where TEntity : Entity
    {
        EntityEntry<TEntity> Handle(TEntity entity);
    }
}
