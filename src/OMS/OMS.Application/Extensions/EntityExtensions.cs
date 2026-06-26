using OMS.Common.Interfaces.Entity;
using OMS.Domain.Abstractions.Events;
using OMS.Domain.Interfaces.Connectors;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Extensions
{
    internal static class EntityExtensions
    {
        internal static IDomainEvent<TEntity> ToDomainEvent<TEntity>(this TEntity entity, IConnector[]?connectors) where TEntity : IEntity<Guid>
            => new CreationDomainEvent<TEntity>(entity, connectors);
    }
}
