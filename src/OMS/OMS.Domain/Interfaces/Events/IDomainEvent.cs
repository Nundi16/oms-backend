using OMS.Common.Abstractions.Entity;

namespace OMS.Domain.Interfaces.Events
{
    public interface IDomainEvent<out TEntity> where TEntity : Entity
    {
        TEntity Entity { get; }
    }
}
