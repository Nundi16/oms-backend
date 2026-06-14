using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record CreationDomainEvent<TEntity>(TEntity Entity)
        : DomainEvent<TEntity>(Entity),
        ICreationDomainEvent<TEntity>
        where TEntity : Entity;
}
