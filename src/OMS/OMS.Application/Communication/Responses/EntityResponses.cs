using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Communication.Responses
{
	/// <summary>
	/// Generic response containing a single entity
	/// </summary>
	/// <typeparam name="TEntity">The entity type</typeparam>
	public sealed record EntityResponse<TEntity>(TEntity Entity) where TEntity : Entity;

	/// <summary>
	/// Generic response containing a list of entities (future: pagination metadata)
	/// </summary>
	/// <typeparam name="TEntity">The entity type</typeparam>
	public sealed record EntityListResponse<TEntity>(IReadOnlyList<TEntity> Entities) where TEntity : Entity;

	/// <summary>
	/// Generic response for delete operations
	/// </summary>
	public sealed record DeleteEntityResponse(Guid Id, bool Success);
}
