using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record ModificationDomainEvent<TEntity>(TEntity Entity)
        : DomainEvent<TEntity>(Entity),
        IModificationDomainEvent<TEntity>
        where TEntity : Entity;
}
