using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Infrastructure.Interfaces.Communication.Handlers
{
    internal interface IModificationEventHandler
    {
        EntityEntry<TEntity> Handle<TDomainEvent, TEntity>(TEntity entity)
            where TDomainEvent : IModificationDomainEvent<TEntity>
            where TEntity : notnull, Entity;
    }
}
