using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record DeletionDomainEvent<TEntity>(TEntity Entity)
        : DomainEvent<TEntity>(Entity),
        IDeletionDomainEvent<TEntity>
        where TEntity : Entity;
}
