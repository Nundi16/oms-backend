using OMS.Common.Interfaces.Entity;

namespace OMS.Domain.Interfaces.Connectors
{
    public interface IConnector : IEntity<Guid>
    {
        string TypeDescriptor { get; }
        Guid SourceId { get; }
        Guid DependantId { get; }
    }
}
