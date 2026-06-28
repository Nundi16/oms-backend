using OMS.Common.Interfaces.Entity;

namespace OMS.Domain.Interfaces.Events
{
    public interface IDomainEvent<out TEntity> where TEntity : IEntity<Guid>
    {
        TEntity Entity { get; }
        IConnector<Guid>[]? Connectors { get; }  
    }
}
