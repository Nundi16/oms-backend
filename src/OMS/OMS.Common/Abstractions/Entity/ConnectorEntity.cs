using System.ComponentModel.DataAnnotations.Schema;
using OMS.Common.Interfaces.Entity;

namespace OMS.Common.Abstractions.Entity
{
    public abstract class ConnectorEntity<TSource, TDependant> : Entity, IConnectorEntity
    {
        public abstract string TypeDescriptor { get; }

        public Guid? SourceId { get; set; }

        public Guid DependantId { get; set; }

        public TSource? Source { get; set; }

        public TDependant? Dependant { get; set; }

        [NotMapped]
        public IConnectorEntity[]? Connectors { get; set; }

        public void AssignSourceId(Guid sourceId)
        {
            SourceId = sourceId;
        }
    }
}
