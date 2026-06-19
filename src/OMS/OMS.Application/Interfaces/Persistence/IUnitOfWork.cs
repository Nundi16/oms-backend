namespace OMS.Application.Interfaces.Persistence
{
	/// <summary>
	/// Application-layer abstraction over the persistence transaction boundary.
	/// Allows command handlers to flush all pending changes (parent entity + connector
	/// modifications scheduled by fan-out handlers) in a single <c>SaveChanges</c> call,
	/// preserving deferred execution.
	/// </summary>
	public interface IUnitOfWork
	{
		/// <summary>
		/// Persists all pending changes accumulated in the current scope.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the operation.</param>
		/// <returns>The number of state entries written.</returns>
		Task<int> SaveAsync(CancellationToken cancellationToken = default);
	}
}
