using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;
using OMS.Infrastructure.Interfaces.Communication.Handlers;

namespace OMS.Infrastructure.Communication.Handlers
{
    internal sealed class DeletionEventHandler(DbContext context) : IDeletionDomainEventHandler
    {
        public EntityEntry<TEntity> Handle<TDomainEvent, TEntity>(TEntity entity)
            where TDomainEvent : IDeletionDomainEvent<TEntity>
            where TEntity : notnull, Entity =>
            context.Remove(entity);
    }
}
