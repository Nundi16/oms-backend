using OMS.Application.Interfaces.Authorization;
using OMS.Common;
using OMS.Common.Communication.Authorization.Guards;
using OMS.Common.Interfaces;
using OMS.Domain.Connectors.OrderClinicConnector;
using OMS.Domain.Modules.ClinicModule;
using OMS.Domain.Modules.OrderModule;
using OMS.Infrastructure.Modules.ConnectorPipeline;

namespace OMS.Infrastructure.Modules.OrderClinicConnector
{
	internal sealed class OrderClinicQueryHandler(
		IUserContext userContext,
		ApplicationDbContext dbContext,
		ICurrentClinicProvider currentClinicProvider)
		: BaseConnectorScopeQueryHandler<Order, Clinic, OrderClinic, ModuleRuleGuard>(
			new ModuleRuleGuard(userContext, Constants.Auth.Roles.CLINIC_ENABLED), dbContext)
	{
		protected override Guid GetScopeDependantId() => currentClinicProvider.GetCurrentClinicId();
	}
}

