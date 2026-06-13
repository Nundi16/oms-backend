using OMS.Common.Abstractions.Entity;

namespace OMS.Domain.Interfaces.Events
{
    public interface IModificationDomainEvent<out TEntity> : IDomainEvent<TEntity> where TEntity : Entity;
}
