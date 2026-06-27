using OMS.Common.Interfaces.Connectors;
using OMS.Common.Interfaces.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record CreationDomainEvent<TEntity>(TEntity Entity, IConnector[]? Connectors, bool PersistChanges = true)
        : DomainEvent<TEntity>(Entity, Connectors),
        ICreationDomainEvent<TEntity>
        where TEntity : IEntity<Guid>;
}
