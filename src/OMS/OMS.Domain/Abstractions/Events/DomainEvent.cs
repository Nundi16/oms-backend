using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public record DomainEvent<TEntity>(TEntity Entity) : IDomainEvent<TEntity> where TEntity : Entity;
}
