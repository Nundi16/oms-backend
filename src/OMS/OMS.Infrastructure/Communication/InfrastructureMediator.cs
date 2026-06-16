using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Interfaces.Communication;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Communication;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Domain.Interfaces.Events;
using OMS.Infrastructure.Interfaces.Communication.Handlers;

namespace OMS.Infrastructure.Communication
{
    internal sealed class InfrastructureMediator(IServiceProvider serviceProvider, IMediatorAuthorizationGuard authorizationGuard) 
        : AuthorizingMediator(serviceProvider, authorizationGuard),
        IInfrastructureMediator
    {
        public async Task<TEntity> HandleCreationAsync<TDomainEvent, TEntity>(TDomainEvent @event, CancellationToken cancellationToken = default)
            where TDomainEvent : ICreationDomainEvent<TEntity>
            where TEntity : Entity
        {
            var handler = ServiceProvider.GetRequiredService<ICreationDomainEventHandler>();

            var result = await handler.HandleAsync<TDomainEvent, TEntity>(@event.Entity, cancellationToken);

            return result.Entity;
        }

        public TEntity HandleDeletion<TDomainEvent, TEntity>(TDomainEvent @event)
            where TDomainEvent : IDeletionDomainEvent<TEntity>
            where TEntity : Entity
        {
            var handler = ServiceProvider.GetRequiredService<IDeletionDomainEventHandler>();

            var result = handler.Handle<TDomainEvent, TEntity>(@event.Entity);

            return result.Entity;
        }

        public TEntity HandleModification<TDomainEvent, TEntity>(TDomainEvent @event)
            where TDomainEvent : IModificationDomainEvent<TEntity>
            where TEntity : Entity
        {
            var handler = ServiceProvider.GetRequiredService<IModificationEventHandler>();

            var result = handler.Handle<TDomainEvent, TEntity>(@event.Entity);

            return result.Entity;
        }
    }
}
