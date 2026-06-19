using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OMS.Application.Connectors;
using OMS.Application.Connectors.Pipeline;
using OMS.Common.Abstractions.Communication.Handlers.Event;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Domain.Connectors;

namespace OMS.Infrastructure.Modules.ConnectorPipeline
{
	/// <summary>
	/// Reusable base for authorized event handlers that participate in the
	/// <see cref="ConnectorPersistContext{TParent}"/> fan-out. Implements the standard
	/// replace-semantics diff (delete stale, insert new, update matched) over an
	/// EF Core <c>DbSet&lt;TConnectorEntity&gt;</c> using AutoMapper to translate
	/// between the connector DTO and the connector entity.
	/// <para>
	/// The handler ONLY schedules <c>Add</c>/<c>Update</c>/<c>Remove</c> on the shared
	/// <see cref="ApplicationDbContext"/>; it never calls <c>SaveChangesAsync</c>. The
	/// parent CRUD handler flushes via <c>IUnitOfWork.SaveAsync</c> once the fan-out
	/// completes, preserving deferred execution.
	/// </para>
	/// </summary>
	/// <typeparam name="TParent">Parent entity type that owns the connectors.</typeparam>
	/// <typeparam name="TDependant">Dependent entity type referenced by the connector.</typeparam>
	/// <typeparam name="TConnectorEntity">Concrete EF-mapped connector entity.</typeparam>
	/// <typeparam name="TConnectorDto">Concrete connector DTO carrying the incoming data.</typeparam>
	/// <typeparam name="TGuard">Authorization guard gating this handler.</typeparam>
	internal abstract class BaseConnectorPersistHandler<TParent, TDependant, TConnectorEntity, TConnectorDto, TGuard>(
		TGuard guard,
		ApplicationDbContext dbContext,
		IMapper mapper)
		: AuthorizedEventHandler<ConnectorPersistContext<TParent>, TGuard>(guard)
		where TParent : Entity
		where TDependant : Entity
		where TConnectorEntity : Connector<TParent, TDependant>, new()
		where TConnectorDto : BaseConnectorDto
		where TGuard : IAuthorizationGuard
	{
		protected ApplicationDbContext DbContext { get; } = dbContext;
		protected IMapper Mapper { get; } = mapper;

		public override async Task HandleAsync(ConnectorPersistContext<TParent> @event, CancellationToken cancellationToken = default)
		{
			var incoming = @event.IncomingConnectors.OfType<TConnectorDto>().ToList();

			var existing = await DbContext.Set<TConnectorEntity>()
				.Where(c => c.ParentId == @event.ParentId)
				.ToListAsync(cancellationToken);

			var incomingIds = incoming
				.Where(dto => dto.Id != Guid.Empty)
				.Select(dto => dto.Id)
				.ToHashSet();

			// Delete: rows that disappeared from the incoming payload (replace semantics).
			foreach (var stale in existing.Where(e => !incomingIds.Contains(e.Id)))
			{
				DbContext.Remove(stale);
			}

			// Insert: incoming entries without an id.
			foreach (var dto in incoming.Where(d => d.Id == Guid.Empty))
			{
				var entity = Mapper.Map<TConnectorEntity>(dto);
				entity.ParentId = @event.ParentId;
				DbContext.Add(entity);
			}

			// Update: incoming entries that match an existing row.
			foreach (var dto in incoming.Where(d => d.Id != Guid.Empty))
			{
				var match = existing.FirstOrDefault(e => e.Id == dto.Id);
				if (match is null)
				{
					continue;
				}

				Mapper.Map(dto, match);
				// Re-pin the parent id; the incoming DTO is allowed to omit/zero it.
				match.ParentId = @event.ParentId;
			}
		}
	}
}
