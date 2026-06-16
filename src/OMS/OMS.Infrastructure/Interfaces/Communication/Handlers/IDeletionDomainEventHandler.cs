using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Communication.Handlers;
using OMS.Domain.Interfaces.Events;

namespace OMS.Infrastructure.Interfaces.Communication.Handlers
{
    internal interface IDeletionDomainEventHandler : IHandler
    {
        EntityEntry<TEntity> Handle<TDomainEvent, TEntity>(TEntity entity)
            where TDomainEvent : IDeletionDomainEvent<TEntity>
            where TEntity : notnull, Entity;
    }
}
