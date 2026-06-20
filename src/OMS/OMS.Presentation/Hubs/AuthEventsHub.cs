using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace OMS.Presentation.Hubs
{
	/// <summary>
	/// SignalR hub that fans out authentication-related events (today: role changes
	/// pushed in by the FusionAuth webhook) to connected frontends. Each client is
	/// routed by the <c>sub</c> claim from its bearer JWT via
	/// <see cref="SubClaimUserIdProvider"/>; the webhook controller then sends to
	/// <c>Clients.User(userId)</c>, so only the affected user's frontends are notified.
	/// </summary>
	[Authorize]
	public sealed class AuthEventsHub : Hub<IAuthEventsClient>
	{
	}
}
