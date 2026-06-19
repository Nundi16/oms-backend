using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OMS.Application.Connectors;
using OMS.Application.Connectors.Abstractions;
using OMS.Application.Connectors.OrderClinicConnector;
using OMS.Domain.Connectors.OrderClinicConnector;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Infrastructure.Modules.OrderClinicConnector
{
	/// <summary>
	/// Writes OrderClinic connector entities to the database using replace semantics:
	/// - Connectors in the incoming list with matching IDs are updated.
	/// - New connectors (ID == Guid.Empty or not found) are inserted.
	/// - Existing connectors not in the incoming list are deleted.
	/// </summary>
	internal sealed class OrderClinicWriter(ApplicationDbContext dbContext, IMapper mapper)
		: IConnectorWriter<Order>
	{
		public async Task WriteAsync(
			Guid parentId,
			IReadOnlyList<BaseConnectorDto> incomingConnectors,
			CancellationToken cancellationToken = default)
		{
			// Filter to only OrderClinicDto instances (polymorphic list may contain other connector types)
			var incomingOrderClinicDtos = incomingConnectors
				.OfType<OrderClinicDto>()
				.ToList();

			// Load existing OrderClinic entities for this parent
			var existingEntities = await dbContext.Set<OrderClinic>()
				.Where(oc => oc.ParentId == parentId)
				.ToListAsync(cancellationToken);

			// Build a set of incoming IDs for quick lookup (excluding default Guid.Empty for new items)
			var incomingIds = incomingOrderClinicDtos
				.Where(dto => dto.Id != Guid.Empty)
				.Select(dto => dto.Id)
				.ToHashSet();

			// Delete entities that are not in the incoming list
			var entitiesToDelete = existingEntities
				.Where(e => !incomingIds.Contains(e.Id))
				.ToList();

			foreach (var entity in entitiesToDelete)
			{
				dbContext.Remove(entity);
			}

			// Upsert incoming items
			foreach (var dto in incomingOrderClinicDtos)
			{
				if (dto.Id == Guid.Empty)
				{
					// New connector: insert
					var newEntity = new OrderClinic
					{
						ParentId = parentId,
						DependantId = dto.DependantId,
						ClinicSpecificOrderName = dto.ClinicSpecificOrderName
					};
					dbContext.Add(newEntity);
				}
				else
				{
					// Existing connector: update
					var existing = existingEntities.FirstOrDefault(e => e.Id == dto.Id);
					if (existing != null)
					{
						existing.DependantId = dto.DependantId;
						existing.ClinicSpecificOrderName = dto.ClinicSpecificOrderName;
					}
				}
			}

			// Save changes for this writer's modifications
			// NOTE: In a true UnitOfWork pattern, this would be deferred to the parent handler.
			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
