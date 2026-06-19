using OMS.Common.Abstractions.Entity;

namespace OMS.Domain.Connectors
{
    public abstract class Connector<TParent, TDependant> : Entity
        where TParent : Entity
        where TDependant : Entity
    {
        public Guid ParentId { get; set; }
        public TParent? Parent { get; set; }
        public Guid DependantId { get; set; }
        public TDependant? Dependant { get; set; }
    }
}
