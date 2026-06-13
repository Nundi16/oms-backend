using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Infrastructure.Interfaces.Communication.Handlers
{
    internal interface ICreationDomainEventHandler
    {
        Task<EntityEntry<TEntity>> HandleAsync<TDomainEvent, TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TDomainEvent : ICreationDomainEvent<TEntity>
            where TEntity : notnull, Entity;
    }
}
