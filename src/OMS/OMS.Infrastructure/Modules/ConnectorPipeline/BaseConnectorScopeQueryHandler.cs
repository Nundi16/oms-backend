using OMS.Application.Connectors.Pipeline;
using OMS.Common.Abstractions.Communication.Handlers.Event;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Domain.Connectors;

namespace OMS.Infrastructure.Modules.ConnectorPipeline
{
	/// <summary>
	/// Reusable base for authorized event handlers that participate in the
	/// <see cref="EntityQueryContext{TParent}"/> fan-out and want to restrict the parent
	/// query to rows that have a connector row pointing to a scope-specific dependent id
	/// (e.g. the current clinic, the current tenant, …).
	/// <para>
	/// The composed filter is a deferred <c>WHERE EXISTS</c> over
	/// <typeparamref name="TConnectorEntity"/>; the query stays unmaterialised so further
	/// connector handlers can keep composing on top of it.
	/// </para>
	/// </summary>
	/// <typeparam name="TParent">Parent entity type being queried.</typeparam>
	/// <typeparam name="TDependant">Dependent entity type referenced by the connector.</typeparam>
	/// <typeparam name="TConnectorEntity">Concrete EF-mapped connector entity.</typeparam>
	/// <typeparam name="TGuard">Authorization guard gating this handler.</typeparam>
	internal abstract class BaseConnectorScopeQueryHandler<TParent, TDependant, TConnectorEntity, TGuard>(
		TGuard guard,
		ApplicationDbContext dbContext)
		: AuthorizedEventHandler<EntityQueryContext<TParent>, TGuard>(guard)
		where TParent : Entity
		where TDependant : Entity
		where TConnectorEntity : Connector<TParent, TDependant>
		where TGuard : IAuthorizationGuard
	{
		protected ApplicationDbContext DbContext { get; } = dbContext;

		/// <summary>
		/// The dependent-side id that the parent query must be restricted to (e.g. current
		/// clinic id). Returning <see cref="Guid.Empty"/> short-circuits the handler so it
		/// adds no filter — guard should have rejected in that case, but this is defensive.
		/// </summary>
		protected abstract Guid GetScopeDependantId();

		public override Task HandleAsync(EntityQueryContext<TParent> @event, CancellationToken cancellationToken = default)
		{
			var dependantId = GetScopeDependantId();
			if (dependantId == Guid.Empty)
			{
				return Task.CompletedTask;
			}

			@event.Query = @event.Query.Where(p =>
				DbContext.Set<TConnectorEntity>().Any(c =>
					c.ParentId == p.Id && c.DependantId == dependantId));

			return Task.CompletedTask;
		}
	}
}
