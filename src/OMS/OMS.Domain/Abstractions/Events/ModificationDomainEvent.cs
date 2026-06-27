using OMS.Common.Interfaces.Entity;
using OMS.Common.Interfaces.Connectors;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record ModificationDomainEvent<TEntity>(TEntity Entity, IConnector[] Connectors)
        : DomainEvent<TEntity>(Entity, Connectors),
        IModificationDomainEvent<TEntity>
        where TEntity : IEntity<Guid>;
}
