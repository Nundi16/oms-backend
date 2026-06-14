using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Interfaces.Communication
{
    public interface IInfrastructureMediator
    {
        Task<TEntity> HandleCreationAsync<TDomainEvent, TEntity>(TDomainEvent @event, CancellationToken cancellationToken = default) 
            where TDomainEvent : ICreationDomainEvent<TEntity>
            where TEntity : Entity;
        TEntity HandleModification<TDomainEvent, TEntity>(TDomainEvent @event)
            where TDomainEvent : IModificationDomainEvent<TEntity>
            where TEntity : Entity;
        TEntity HandleDeletion<TDomainEvent, TEntity>(TDomainEvent @event)
            where TDomainEvent : IDeletionDomainEvent<TEntity>
            where TEntity : Entity;
    }
}
