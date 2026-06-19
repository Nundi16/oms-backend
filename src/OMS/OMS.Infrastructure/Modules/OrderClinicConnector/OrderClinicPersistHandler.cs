using AutoMapper;
using OMS.Application.Connectors.OrderClinicConnector;
using OMS.Domain.Connectors.OrderClinicConnector;
using OMS.Domain.Modules.ClinicModule;
using OMS.Domain.Modules.OrderModule;
using OMS.Infrastructure.Authorization;
using OMS.Infrastructure.Modules.ConnectorPipeline;

namespace OMS.Infrastructure.Modules.OrderClinicConnector
{
	/// <summary>
	/// OrderClinic-specific binding of <see cref="BaseConnectorPersistHandler{TParent,TDependant,TConnectorEntity,TConnectorDto,TGuard}"/>.
	/// All persistence logic (diff, replace, upsert) lives in the base; this leaf merely
	/// picks the concrete generic arguments and the authorization guard.
	/// </summary>
	internal sealed class OrderClinicPersistHandler(
		ClinicMembershipGuard guard,
		ApplicationDbContext dbContext,
		IMapper mapper)
		: BaseConnectorPersistHandler<Order, Clinic, OrderClinic, OrderClinicDto, ClinicMembershipGuard>(guard, dbContext, mapper);
}

