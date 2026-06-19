using OMS.Common.Abstractions.Entity;

namespace OMS.Domain.Connectors
{
    public abstract class Connector<TParent, TDependant> : Entity
        where TParent : Entity
        where TDependant : Entity
    {
        /// <summary>
        /// Foreign key to the parent entity (the "owner" side of the M:N relation).
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Foreign key to the dependent entity (the "related" side of the M:N relation).
        /// </summary>
        public Guid DependantId { get; set; }

        /// <summary>
        /// Navigation property to the parent entity. Nullable to support FK-only updates.
        /// </summary>
        public TParent? Parent { get; set; }

        /// <summary>
        /// Navigation property to the dependent entity. Nullable to support FK-only updates.
        /// </summary>
        public TDependant? Dependant { get; set; }
    }
}
