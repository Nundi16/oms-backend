using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;
using OMS.Infrastructure.Interfaces.Communication.Handlers;

namespace OMS.Infrastructure.Communication.Handlers
{
    internal sealed class CreationEventHandler(ApplicationDbContext context) : ICreationDomainEventHandler
    {
        public Task<EntityEntry<TEntity>> HandleAsync<TDomainEvent, TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TDomainEvent : ICreationDomainEvent<TEntity>
            where TEntity : notnull, Entity =>
            context.AddAsync(entity, cancellationToken).AsTask();
    }
}
