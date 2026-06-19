using OMS.Application.Interfaces.Authorization;

namespace OMS.Infrastructure.Authorization
{
	/// <summary>
	/// POC implementation of ICurrentClinicProvider that returns a static clinic ID.
	/// In production, this would be resolved from user claims, session, or HTTP context.
	/// </summary>
	internal sealed class StaticCurrentClinicProvider : ICurrentClinicProvider
	{
		// TODO: Replace with actual user context resolution
		private static readonly Guid PocClinicId = Guid.Parse("019ee0b1-9030-72f7-b887-b4b5612427fb");

		public Guid GetCurrentClinicId()
		{
			return PocClinicId;
		}
	}
}
