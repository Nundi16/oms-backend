using OMS.Common.Interfaces.Entity;
using OMS.Domain.Interfaces.Connectors;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record CreationDomainEvent<TEntity>(TEntity Entity, IConnector[]?Connectors)
        : DomainEvent<TEntity>(Entity, Connectors),
        ICreationDomainEvent<TEntity>
        where TEntity : IEntity<Guid>;
}
