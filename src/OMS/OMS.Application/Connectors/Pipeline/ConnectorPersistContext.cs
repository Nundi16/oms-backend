using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Connectors.Pipeline
{
	/// <summary>
	/// Mutable context event fanned out via <c>IMediator.FanOutSequentialAsync</c> after a
	/// parent entity has been created/updated. Connector-specific authorized event handlers
	/// inspect <see cref="IncomingConnectors"/>, pick the DTO types they understand, and
	/// schedule <c>Add</c>/<c>Update</c>/<c>Remove</c> operations on the shared
	/// <c>ApplicationDbContext</c>. The handlers MUST NOT call <c>SaveChangesAsync</c> —
	/// the parent CRUD handler issues a single <c>IUnitOfWork.SaveAsync</c> after the
	/// fan-out completes, preserving deferred execution.
	/// </summary>
	/// <typeparam name="TParent">The parent entity type that owns the connectors.</typeparam>
	public sealed class ConnectorPersistContext<TParent> where TParent : Entity
	{
		public ConnectorPersistContext(Guid parentId, IReadOnlyList<BaseConnectorDto> incomingConnectors)
		{
			ArgumentNullException.ThrowIfNull(incomingConnectors);
			ParentId = parentId;
			IncomingConnectors = incomingConnectors;
		}

		/// <summary>
		/// The id of the parent entity the connectors belong to.
		/// </summary>
		public Guid ParentId { get; }

		/// <summary>
		/// The polymorphic list of incoming connector DTOs received from the client.
		/// Handlers filter by their concrete DTO type.
		/// </summary>
		public IReadOnlyList<BaseConnectorDto> IncomingConnectors { get; }
	}
}
