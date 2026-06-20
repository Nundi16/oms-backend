namespace OMS.Presentation.Hubs
{
	/// <summary>
	/// Notification sent from the API to connected frontends over the
	/// <see cref="AuthEventsHub"/>. Carries enough context for the client to decide
	/// whether to silently refresh its access token (and re-render permission-gated UI)
	/// or to prompt the user to sign in again.
	/// </summary>
	/// <param name="Type">Semantic type of the auth event, e.g. <c>RoleChanged</c>.</param>
	/// <param name="UserId">FusionAuth user id (the <c>sub</c> claim) the event refers to.</param>
	/// <param name="Source">Raw FusionAuth event type that triggered this notification.</param>
	/// <param name="OccurredAt">Server timestamp when the notification was emitted.</param>
	public sealed record AuthEventNotification(
		string Type,
		Guid UserId,
		string Source,
		DateTimeOffset OccurredAt);
}
