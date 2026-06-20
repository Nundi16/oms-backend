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
	/// <summary>
	/// OrderClinic-specific binding of
	/// <see cref="BaseConnectorScopeQueryHandler{TParent,TDependant,TConnectorEntity,TGuard}"/>.
	/// Provides the current clinic id as the scope dependant; the base composes the
	/// deferred <c>WHERE EXISTS</c> filter over <see cref="OrderClinic"/>.
	/// <para>
	/// Authorization: gated by <see cref="ModuleRuleGuard"/> requiring the
	/// <c>clinic_enabled</c> role. Principals without that role are silently skipped
	/// by <see cref="OMS.Common.Communication.AuthorizingMediator"/> during fan-out,
	/// so the Order list query is NOT narrowed to a clinic for them.
	/// </para>
	/// </summary>
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

