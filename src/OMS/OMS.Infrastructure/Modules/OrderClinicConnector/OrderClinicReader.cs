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
	/// Reads OrderClinic connector entities from the database and maps them to OrderClinicDto instances.
	/// Batch-loads connectors for multiple parent Order IDs in a single query.
	/// </summary>
	internal sealed class OrderClinicReader(ApplicationDbContext dbContext, IMapper mapper)
		: IConnectorReader<Order>
	{
		public async Task<IReadOnlyDictionary<Guid, IReadOnlyList<BaseConnectorDto>>> LoadAsync(
			IReadOnlyCollection<Guid> parentIds,
			CancellationToken cancellationToken = default)
		{
			if (parentIds == null || parentIds.Count == 0)
			{
				return new Dictionary<Guid, IReadOnlyList<BaseConnectorDto>>();
			}

			// Batch load all OrderClinic rows for the given parent IDs in a single query
			var rows = await dbContext.Set<OrderClinic>()
				.Where(oc => parentIds.Contains(oc.ParentId))
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			// Group by ParentId and map to DTOs
			var result = rows
				.GroupBy(oc => oc.ParentId)
				.ToDictionary(
					g => g.Key,
					g => (IReadOnlyList<BaseConnectorDto>)g
						.Select(mapper.Map<OrderClinicDto>)
						.ToList());

			return result;
		}
	}
}
