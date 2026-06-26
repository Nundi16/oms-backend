using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Connectors;

namespace OMS.Domain.Connectors
{
    public abstract class Connector<TSource, TDependant> : Entity, IConnector
        where TSource : Entity
        where TDependant : Entity
    {
        public abstract string TypeDescriptor { get; }
        public Guid? SourceId { get; set; }
        public TSource? Source { get; set; }
        public Guid DependantId { get; set; }
        public TDependant? Dependant { get; set; }

    public void AssignSourceId(Guid sourceId) => SourceId = sourceId;
    }
}
