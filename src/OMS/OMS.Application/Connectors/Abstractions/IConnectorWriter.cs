using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Connectors.Abstractions
{
	/// <summary>
	/// Provides a contract for persisting connector DTOs for a given parent entity type.
	/// Implementations handle the replace semantics: connectors present in the incoming list are upserted,
	/// and connectors of the same type not present in the list are deleted.
	/// </summary>
	/// <typeparam name="TParent">The parent entity type that owns the connectors.</typeparam>
	public interface IConnectorWriter<TParent> where TParent : Entity
	{
		/// <summary>
		/// Asynchronously writes (upserts/deletes) connector entities for the specified parent ID.
		/// </summary>
		/// <param name="parentId">The ID of the parent entity.</param>
		/// <param name="incomingConnectors">
		/// The list of connector DTOs representing the desired state. Connectors with a matching ID
		/// are updated; new connectors (ID == Guid.Empty or not found) are inserted; existing connectors
		/// of the writer's type not in this list are deleted.
		/// </param>
		/// <param name="cancellationToken">A token to cancel the operation.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task WriteAsync(
			Guid parentId,
			IReadOnlyList<BaseConnectorDto> incomingConnectors,
			CancellationToken cancellationToken = default);
	}
}
