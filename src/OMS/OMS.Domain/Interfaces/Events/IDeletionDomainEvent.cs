using OMS.Common.Interfaces.Entity;

namespace OMS.Domain.Interfaces.Events
{
    public interface IDeletionDomainEvent<out TEntity> : IDomainEvent<TEntity> where TEntity : IEntity<Guid>
    {
        bool PersistChanges { get; }
    }
}
