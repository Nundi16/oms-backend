using OMS.Common.Abstractions.Entity;

namespace OMS.Domain.Interfaces.Events
{
    public interface IDeletionDomainEvent<out TEntity> : IDomainEvent<TEntity> where TEntity : Entity;
}
