using OMS.Common.Interfaces.Entity;
using OMS.Domain.Abstractions.Events;

namespace OMS.Application.Extensions
{
    internal static class EntityExtenions
    {
        internal static CreationDomainEvent<TEntity> ToCreationDomainEvent<TEntity>(this TEntity entity, IConnector<Guid>[]? connectors = null) 
            where TEntity : class, IEntity<Guid> =>
            new(entity, connectors);

        internal static ModificationDomainEvent<TEntity> ToModificationDomainEvent<TEntity>(this TEntity entity, IConnector<Guid>[]? connectors = null)
            where TEntity : class, IEntity<Guid> =>
            new(entity, connectors);

        internal static DeletionDomainEvent<TEntity> ToDeletionDomainEvent<TEntity>(this TEntity entity, IConnector<Guid>[]? connectors = null)
            where TEntity : class, IEntity<Guid> =>
            new(entity, connectors);
    }
}
