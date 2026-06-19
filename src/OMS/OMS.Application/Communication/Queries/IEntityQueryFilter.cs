using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Communication.Queries
{
	/// <summary>
	/// Provides a contract for applying cross-cutting query filters to entity queryables.
	/// Implementations modify the query in a deferred manner (no database execution),
	/// allowing multiple filters to be composed and translated into a single SQL statement.
	/// </summary>
	/// <typeparam name="TEntity">The entity type to filter.</typeparam>
	public interface IEntityQueryFilter<TEntity> where TEntity : Entity
	{
		/// <summary>
		/// Applies a filter to the given queryable.
		/// </summary>
		/// <param name="query">The queryable to filter. Must be deferred (not yet executed).</param>
		/// <returns>A new queryable with the filter applied, still deferred.</returns>
		IQueryable<TEntity> Apply(IQueryable<TEntity> query);
	}
}
