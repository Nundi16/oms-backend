using OMS.Common.Interfaces.Entity;
using OMS.Domain.Connectors;
using OMS.Common.Interfaces.Connectors;

namespace OMS.Domain.Interfaces.Events
{
    public interface IDomainEvent<out TEntity> where TEntity : IEntity<Guid>
    {
        TEntity Entity { get; }
        IConnector[]?Connectors { get; }  
    }
}
