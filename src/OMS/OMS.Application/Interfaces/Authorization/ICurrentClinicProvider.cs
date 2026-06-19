namespace OMS.Application.Interfaces.Authorization
{
	/// <summary>
	/// Provides access to the current clinic context for query filtering and authorization.
	/// In POC/testing scenarios, this may return a static value; in production, it would
	/// be resolved from user claims, session, or other identity sources.
	/// </summary>
	public interface ICurrentClinicProvider
	{
		/// <summary>
		/// Gets the ID of the clinic associated with the current request context.
		/// </summary>
		/// <returns>The clinic ID.</returns>
		Guid GetCurrentClinicId();
	}
}
