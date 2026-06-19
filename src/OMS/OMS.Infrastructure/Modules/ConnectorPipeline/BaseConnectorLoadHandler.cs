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
	/// <see cref="ConnectorLoadContext{TParent}"/> fan-out. Batch-loads
	/// <typeparamref name="TConnectorEntity"/> rows for every requested parent id, maps
	/// them to <typeparamref name="TConnectorDto"/> with AutoMapper, and appends them
	/// to the shared accumulator keyed by parent id. The CRUD handler then attaches the
	/// accumulated DTOs to the carrier parent DTOs.
	/// </summary>
	/// <typeparam name="TParent">Parent entity type whose connectors are being loaded.</typeparam>
	/// <typeparam name="TDependant">Dependent entity type referenced by the connector.</typeparam>
	/// <typeparam name="TConnectorEntity">Concrete EF-mapped connector entity.</typeparam>
	/// <typeparam name="TConnectorDto">Concrete connector DTO produced for the response.</typeparam>
	/// <typeparam name="TGuard">Authorization guard gating this handler.</typeparam>
	internal abstract class BaseConnectorLoadHandler<TParent, TDependant, TConnectorEntity, TConnectorDto, TGuard>(
		TGuard guard,
		ApplicationDbContext dbContext,
		IMapper mapper)
		: AuthorizedEventHandler<ConnectorLoadContext<TParent>, TGuard>(guard)
		where TParent : Entity
		where TDependant : Entity
		where TConnectorEntity : Connector<TParent, TDependant>
		where TConnectorDto : BaseConnectorDto
		where TGuard : IAuthorizationGuard
	{
		protected ApplicationDbContext DbContext { get; } = dbContext;
		protected IMapper Mapper { get; } = mapper;

		public override async Task HandleAsync(ConnectorLoadContext<TParent> @event, CancellationToken cancellationToken = default)
		{
			if (@event.ParentIds.Count == 0)
			{
				return;
			}

			var rows = await DbContext.Set<TConnectorEntity>()
				.Where(c => @event.ParentIds.Contains(c.ParentId))
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			foreach (var row in rows)
			{
				var dto = Mapper.Map<TConnectorDto>(row);
				if (!@event.Accumulator.TryGetValue(row.ParentId, out var bucket))
				{
					bucket = new List<BaseConnectorDto>();
					@event.Accumulator[row.ParentId] = bucket;
				}
				bucket.Add(dto);
			}
		}
	}
}
