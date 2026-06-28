using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record DomainEvent<TEntity>(TEntity Entity, IConnector<Guid>[]? Connectors, bool SaveChanges = false) : IDomainEvent<TEntity> where TEntity : IEntity<Guid>;
}
