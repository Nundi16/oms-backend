using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Entity;

namespace OMS.Common.Interfaces.Connectors
{
    public interface IConnector : IEntity<Guid>
    {
        string TypeDescriptor { get; }
        Guid? SourceId { get; }
        Guid DependantId { get; }
        IConnector[]? ChildConnectors { get; set; }

        void AssignSourceId(Guid sourceId);
    }
}
