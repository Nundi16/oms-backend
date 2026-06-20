using Microsoft.AspNetCore.SignalR;

namespace OMS.Presentation.Hubs
{
	/// <summary>
	/// Provides the SignalR user identifier used by <c>Clients.User(...)</c>. We disable
	/// inbound-claim mapping in the JwtBearer pipeline, so the token's <c>sub</c> claim
	/// is not promoted to <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>;
	/// the default <c>DefaultUserIdProvider</c> would return <c>null</c> for us.
	/// </summary>
	internal sealed class SubClaimUserIdProvider : IUserIdProvider
	{
		public string? GetUserId(HubConnectionContext connection)
			=> connection.User?.FindFirst("sub")?.Value;
	}
}
