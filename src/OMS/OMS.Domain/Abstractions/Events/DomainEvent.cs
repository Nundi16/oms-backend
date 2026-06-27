using OMS.Common.Interfaces.Entity;
using OMS.Common.Interfaces.Connectors;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record DomainEvent<TEntity>(TEntity Entity, IConnector[]?Connectors) : IDomainEvent<TEntity> where TEntity : IEntity<Guid>;
}
