using OMS.Application.Communication.Queries;
using OMS.Application.Interfaces.Authorization;
using OMS.Domain.Connectors.OrderClinicConnector;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Infrastructure.Modules.OrderClinicConnector
{
	/// <summary>
	/// Query filter that restricts Order queries to those associated with the current clinic
	/// via the OrderClinic connector. Applied as a deferred WHERE EXISTS subquery.
	/// </summary>
	internal sealed class OrderClinicCurrentClinicFilter(
		ApplicationDbContext dbContext,
		ICurrentClinicProvider currentClinicProvider)
		: IEntityQueryFilter<Order>
	{
		public IQueryable<Order> Apply(IQueryable<Order> query)
		{
			var clinicId = currentClinicProvider.GetCurrentClinicId();

			// Apply EXISTS subquery: only orders connected to the current clinic
			return query.Where(o =>
				dbContext.Set<OrderClinic>().Any(oc =>
					oc.ParentId == o.Id && oc.DependantId == clinicId));
		}
	}
}
