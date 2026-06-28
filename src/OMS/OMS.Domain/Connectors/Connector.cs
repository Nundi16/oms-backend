using System.ComponentModel.DataAnnotations.Schema;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Connectors;
using OMS.Common.Models;
using OMS.Domain.Abstractions.Events;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Connectors
{
    
    public abstract class Connector<TSelf,TSource, TDependant> : Entity, IConnector
        where TSelf : Connector<TSelf, TSource, TDependant> // 4Robi: https://en.wikipedia.org/wiki/Curiously_recurring_template_pattern
        where TSource : Entity
        where TDependant : Entity
    {
        public abstract string TypeDescriptor { get; }
        public Guid? SourceId { get; set; }
        public TSource? Source { get; set; }
        public Guid DependantId { get; set; }
        public TDependant? Dependant { get; set; }
        [NotMapped]
        public  IConnector[]? ChildConnectors { get; set; }

        public void AssignSourceId(Guid sourceId) => SourceId = sourceId;
    }
}


