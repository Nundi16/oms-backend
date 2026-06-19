using OMS.Application.Interfaces.Authorization;
using OMS.Common;
using OMS.Common.Interfaces;
using OMS.Common.Abstractions.Communication.Authorization.Guards;

namespace OMS.Infrastructure.Authorization
{
	/// <summary>
	/// POC guard that authorizes a handler iff the current request has a resolvable
	/// clinic context. Real implementation would inspect user roles / clinic-membership
	/// claims; here we simply require <see cref="ICurrentClinicProvider"/> to return a
	/// non-empty <see cref="Guid"/>.
	/// </summary>
	internal sealed class ClinicMembershipGuard(IUserContext userContext, ICurrentClinicProvider currentClinicProvider)
		: AuthorizationGuard(userContext)
	{
		// Base.Authorize() compares Claims.FindFirst(RequiredClaimType); we override
		// the public API instead so the POC works without a real ClaimsPrincipal.
		protected override string RequiredClaimType => "clinic_membership";

		public override IResult Authorize()
		{
			var clinicId = currentClinicProvider.GetCurrentClinicId();
			return clinicId != Guid.Empty
				? Result.Success()
				: Result.Failure("No current clinic in context.");
		}
	}
}
