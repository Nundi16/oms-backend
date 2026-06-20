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
	/// <summary>
	/// OrderClinic-specific binding of <see cref="BaseConnectorLoadHandler{TParent,TDependant,TConnectorEntity,TConnectorDto,TGuard}"/>.
	/// All batch-loading and accumulator logic lives in the base; this leaf merely picks
	/// the concrete generic arguments and the authorization guard.
	/// <para>
	/// Authorization: gated by <see cref="ModuleRuleGuard"/> requiring the
	/// <c>clinic_enabled</c> role. Principals without that role are silently skipped
	/// by <see cref="OMS.Common.Communication.AuthorizingMediator"/> during fan-out,
	/// so the parent Order load completes without clinic links.
	/// </para>
	/// </summary>
	internal sealed class OrderClinicLoadHandler(
		IUserContext userContext,
		ApplicationDbContext dbContext,
		IMapper mapper)
		: BaseConnectorLoadHandler<Order, Clinic, OrderClinic, OrderClinicDto, ModuleRuleGuard>(
			new ModuleRuleGuard(userContext, Constants.Auth.Roles.CLINIC_ENABLED), dbContext, mapper);
}

