using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;
using OMS.Infrastructure.Interfaces.Communication.Handlers;

namespace OMS.Infrastructure.Communication.Handlers
{
    internal sealed class ModificationEventHandler(DbContext context) : IModificationEventHandler
    {
        public EntityEntry<TEntity> Handle<TDomainEvent, TEntity>(TEntity entity)
            where TDomainEvent : IModificationDomainEvent<TEntity>
            where TEntity : notnull, Entity =>
            context.Update(entity);
    }
}
