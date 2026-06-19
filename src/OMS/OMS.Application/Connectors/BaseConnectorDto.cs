namespace OMS.Application.Connectors
{
    /// <summary>
    /// Base class for all connector DTOs. Represents a many-to-many relationship between
    /// a parent entity and a dependent entity. Derived classes add connector-specific fields.
    /// </summary>
    public abstract class BaseConnectorDto
    {
        public virtual string Descriptor { get; }
        /// <summary>
        /// The unique identifier of this connector instance.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// The ID of the parent entity (the "owner" side of the relation).
        /// </summary>
        public Guid ParentId { get; init; }

        /// <summary>
        /// The ID of the dependent entity (the "related" side of the relation).
        /// </summary>
        public Guid DependantId { get; init; }
    }
}
