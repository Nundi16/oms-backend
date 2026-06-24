using OMS.Common.Interfaces.Entity;

namespace OMS.Domain.Interfaces.Events
{
    public interface ICreationDomainEvent<out TEntity> : IDomainEvent<TEntity> where TEntity : IEntity<Guid>;
}
