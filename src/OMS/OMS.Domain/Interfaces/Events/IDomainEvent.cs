using OMS.Common.Interfaces.Entity;
using OMS.Domain.Connectors;
using OMS.Common.Interfaces.Connectors;
using OMS.Common.Interfaces.Extensions;

namespace OMS.Domain.Interfaces.Events
{
    public interface IDomainEvent<out TEntity> where TEntity : IEntity<Guid>
    {
        TEntity Entity { get; }
        IConnector[]?Connectors { get; }  
        IExtension[]? Extensions { get; }
    }
}
