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
		private static readonly Guid PocClinicId = Guid.Parse("019edc9f-a5ef-72b4-9350-0e1f14d32e2b");

		public Guid GetCurrentClinicId()
		{
			return PocClinicId;
		}
	}
}
