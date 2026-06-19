using OMS.Application.Interfaces.Authorization;
using OMS.Domain.Connectors.OrderClinicConnector;
using OMS.Domain.Modules.ClinicModule;
using OMS.Domain.Modules.OrderModule;
using OMS.Infrastructure.Authorization;
using OMS.Infrastructure.Modules.ConnectorPipeline;

namespace OMS.Infrastructure.Modules.OrderClinicConnector
{
	/// <summary>
	/// OrderClinic-specific binding of
	/// <see cref="BaseConnectorScopeQueryHandler{TParent,TDependant,TConnectorEntity,TGuard}"/>.
	/// Provides the current clinic id as the scope dependant; the base composes the
	/// deferred <c>WHERE EXISTS</c> filter over <see cref="OrderClinic"/>.
	/// </summary>
	internal sealed class OrderClinicQueryHandler(
		ClinicMembershipGuard guard,
		ApplicationDbContext dbContext,
		ICurrentClinicProvider currentClinicProvider)
		: BaseConnectorScopeQueryHandler<Order, Clinic, OrderClinic, ClinicMembershipGuard>(guard, dbContext)
	{
		protected override Guid GetScopeDependantId() => currentClinicProvider.GetCurrentClinicId();
	}
}

