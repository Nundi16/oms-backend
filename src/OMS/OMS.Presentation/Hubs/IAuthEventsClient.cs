namespace OMS.Presentation.Hubs
{
	/// <summary>
	/// Strongly-typed client contract for <see cref="AuthEventsHub"/>. Frontends must
	/// subscribe to <c>AuthEvent</c> on their hub connection to receive notifications.
	/// </summary>
	public interface IAuthEventsClient
	{
		Task AuthEvent(AuthEventNotification notification);
	}
}
