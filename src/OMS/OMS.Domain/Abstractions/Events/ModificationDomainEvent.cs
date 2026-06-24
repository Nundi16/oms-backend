using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Connectors;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record ModificationDomainEvent<TEntity>(TEntity Entity, IConnector[] Connectors)
        : DomainEvent<TEntity>(Entity, Connectors),
        IModificationDomainEvent<TEntity>
        where TEntity : Entity;
}
