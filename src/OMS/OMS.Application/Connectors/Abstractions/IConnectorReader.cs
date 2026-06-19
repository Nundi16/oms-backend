using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Connectors.Abstractions
{
	/// <summary>
	/// Provides a contract for loading connector DTOs for a given parent entity type.
	/// Implementations batch-load connector data from the database for multiple parent IDs
	/// and return them grouped by parent ID.
	/// </summary>
	/// <typeparam name="TParent">The parent entity type that owns the connectors.</typeparam>
	public interface IConnectorReader<TParent> where TParent : Entity
	{
		/// <summary>
		/// Asynchronously loads connector DTOs for the specified parent IDs.
		/// </summary>
		/// <param name="parentIds">The collection of parent entity IDs to load connectors for.</param>
		/// <param name="cancellationToken">A token to cancel the operation.</param>
		/// <returns>
		/// A read-only dictionary where the key is the parent ID and the value is a list of connector DTOs
		/// associated with that parent. Parents with no connectors may be absent from the dictionary.
		/// </returns>
		Task<IReadOnlyDictionary<Guid, IReadOnlyList<BaseConnectorDto>>> LoadAsync(
			IReadOnlyCollection<Guid> parentIds,
			CancellationToken cancellationToken = default);
	}
}
