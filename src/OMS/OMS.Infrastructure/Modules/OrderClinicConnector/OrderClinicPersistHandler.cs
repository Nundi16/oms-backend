using AutoMapper;
using OMS.Application.Connectors.OrderClinicConnector;
using OMS.Common;
using OMS.Common.Communication.Authorization.Guards;
using OMS.Common.Interfaces;
using OMS.Domain.Connectors.OrderClinicConnector;
using OMS.Domain.Modules.ClinicModule;
using OMS.Domain.Modules.OrderModule;
using OMS.Infrastructure.Modules.ConnectorPipeline;

namespace OMS.Infrastructure.Modules.OrderClinicConnector
{
	internal sealed class OrderClinicPersistHandler(
		IUserContext userContext,
		ApplicationDbContext dbContext,
		IMapper mapper)
		: BaseConnectorPersistHandler<Order, Clinic, OrderClinic, OrderClinicDto, ModuleRuleGuard>(
			new ModuleRuleGuard(userContext, Constants.Auth.Roles.CLINIC_ENABLED), dbContext, mapper);
}

