using OMS.Application.Models;
using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Communication.Requests
{
	/// <summary>
	/// Generic request for creating an entity
	/// </summary>
	/// <typeparam name="TEntity">The entity type to create</typeparam>
	public sealed record CreateEntityRequest<TEntity>(TEntity Entity) where TEntity : Entity;

	/// <summary>
	/// Generic request for updating an entity
	/// </summary>
	/// <typeparam name="TEntity">The entity type to update</typeparam>
	public sealed record UpdateEntityRequest<TEntity>(Guid Id, TEntity Entity) where TEntity : Entity;

	/// <summary>
	/// Generic request for deleting an entity
	/// </summary>
	/// <typeparam name="TEntity">The entity type to delete</typeparam>
	public sealed record DeleteEntityRequest<TEntity>(Guid Id) where TEntity : Entity;

	/// <summary>
	/// Generic request for getting a single entity by ID
	/// </summary>
	/// <typeparam name="TEntity">The entity type to retrieve</typeparam>
	public sealed record GetEntityRequest<TEntity>(Guid Id) where TEntity : Entity;

	/// <summary>
	/// Generic request for getting a list of entities (future: pagination, filtering, sorting)
	/// </summary>
	/// <typeparam name="TEntity">The entity type to retrieve</typeparam>
	public sealed record GetEntitiesRequest<TEntity>(IList<Filter> Filters) where TEntity : Entity;
}
