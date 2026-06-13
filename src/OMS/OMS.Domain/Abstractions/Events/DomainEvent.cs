using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public abstract record DomainEvent<TEntity> : IDomainEvent<TEntity> where TEntity : Entity
    {
        public required TEntity Entity { get; init; }
    }
}
