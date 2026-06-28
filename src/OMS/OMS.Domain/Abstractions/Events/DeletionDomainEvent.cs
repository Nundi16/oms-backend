using OMS.Common.Interfaces.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record DeletionDomainEvent<TEntity>(TEntity Entity, IConnector<Guid>[]? Connectors)
        : DomainEvent<TEntity>(Entity, Connectors),
        IDeletionDomainEvent<TEntity>
        where TEntity : IEntity<Guid>;
}
