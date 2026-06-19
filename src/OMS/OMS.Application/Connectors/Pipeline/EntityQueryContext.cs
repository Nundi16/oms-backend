using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Connectors.Pipeline
{
	/// <summary>
	/// Mutable context event fanned out via <c>IMediator.FanOutSequentialAsync</c> so that
	/// connector-specific authorized event handlers can compose deferred LINQ filters
	/// over an entity query (e.g. current-clinic restriction). Each handler that passes
	/// its authorization guard reads <see cref="Query"/>, applies <c>Where</c>/<c>Join</c>
	/// expressions, and writes the new <see cref="IQueryable{TEntity}"/> back to it.
	/// The CRUD handler executes <see cref="Query"/> only after every connector handler
	/// has finished, preserving deferred execution.
	/// </summary>
	/// <typeparam name="TEntity">The root entity type being queried.</typeparam>
	public sealed class EntityQueryContext<TEntity> where TEntity : Entity
	{
		public EntityQueryContext(IQueryable<TEntity> query)
		{
			ArgumentNullException.ThrowIfNull(query);
			Query = query;
		}

		/// <summary>
		/// The current deferred query. Handlers replace this with a further-filtered queryable.
		/// </summary>
		public IQueryable<TEntity> Query { get; set; }
	}
}
