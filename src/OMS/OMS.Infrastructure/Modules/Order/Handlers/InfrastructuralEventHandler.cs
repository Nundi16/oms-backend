using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMS.Domain;
using OMS.Infrastructure.Interfaces.Handler;

namespace OMS.Infrastructure.Modules.Handlers
{
    internal class InfrastructuralEventHandler<TEntity>(DbContext context) : IAsyncInfrastructuralEventHandler<TEntity> where TEntity : Entity
    {
        public Task<EntityEntry<TEntity>> HandleAsync(TEntity entity, CancellationToken cancellationToken = default) =>
            context.AddAsync(entity, cancellationToken).AsTask();
    }

    internal class ModificationEventHandler<TEntity>(DbContext context) : IInfrastructuralEventHandler<TEntity> where TEntity : Entity
    {
        public EntityEntry<TEntity> Handle(TEntity entity) =>
            context.Update(entity);
    }

    internal class RemovalEventHandler<TEntity>(DbContext context) : IInfrastructuralEventHandler<TEntity> where TEntity : Entity
    {
        public EntityEntry<TEntity> Handle(TEntity entity) =>
            context.Remove(entity);
    }
}
