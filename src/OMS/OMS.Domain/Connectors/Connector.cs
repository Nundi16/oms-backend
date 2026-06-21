using OMS.Common.Abstractions.Entity;
using OMS.Common.Attributes;
using OMS.Common.Filter.Enums;

namespace OMS.Domain.Connectors
{
    public abstract class Connector<TParent, TDependant> : Entity
        where TParent : Entity
        where TDependant : Entity
    {
        public Guid ParentId { get; set; }
        public TParent? Parent { get; set; }
        [Filterable(nameof(DependantId), FilterOperator.Equals)]
        public Guid DependantId { get; set; }
        public TDependant? Dependant { get; set; }
    }
}
