using OMS.Common.Interfaces;

namespace OMS.Domain.Connectors
{
    public abstract class Connector<TParent, TDependant> : Entity
        where TParent : IDomainEntity, new()
        where TDependant : IDomainEntity, new()
    {
        public TParent Parent { get; set; }
        public TDependant Dependant { get; set; }
    }
}
