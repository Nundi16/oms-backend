using System.Security.Claims;
using OMS.Common.Interfaces;

namespace OMS.Presentation.Authorization
{
	/// <summary>
	/// <see cref="IUserContext"/> implementation backed by the current HTTP request.
	/// Surfaces the FusionAuth-issued JWT claims to the rest of the system.
	/// </summary>
	internal sealed class HttpUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
	{
		public Guid Id
		{
			get
			{
				// FusionAuth sets "sub" to the user's GUID. MapInboundClaims is disabled
				// in the auth pipeline, so the claim type is the raw "sub" value.
				var subject = Claims.FindFirst("sub")?.Value
					?? Claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				return Guid.TryParse(subject, out var id) ? id : Guid.Empty;
			}
		}

		public ClaimsPrincipal Claims => httpContextAccessor.HttpContext?.User ?? _anonymous;

		private static readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
	}
}
