using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using OMS.Presentation.Hubs;

namespace OMS.Presentation.Webhooks.FusionAuth
{
	/// <summary>
	/// Receives FusionAuth webhook callbacks at <c>POST /webhooks/auth-events</c>.
	/// Authenticates the caller via a shared secret in the <c>Authorization</c>
	/// header (configured on the FusionAuth Webhook screen) and, for events that
	/// can mutate a user's roles, fans a refresh hint out to that user's frontends
	/// through <see cref="AuthEventsHub"/>.
	/// </summary>
	[ApiController]
	[Route("webhooks/auth-events")]
	[AllowAnonymous]
	public sealed class FusionAuthWebhookController(
		IOptions<FusionAuthWebhookOptions> options,
		IHubContext<AuthEventsHub, IAuthEventsClient> hubContext,
		TimeProvider timeProvider,
		ILogger<FusionAuthWebhookController> logger) : ControllerBase
	{
		// FusionAuth event types that change a user's effective role set.
		// Other event types are accepted (200 OK) but not fanned out.
		private static readonly HashSet<string> RoleAffectingEvents = new(StringComparer.Ordinal)
		{
			"user.registration.create",
			"user.registration.update",
			"user.registration.delete",
		};

		[HttpPost]
		public async Task<IActionResult> Receive(
			[FromBody] FusionAuthWebhookEnvelope payload,
			CancellationToken cancellationToken)
		{
			if (!TryAuthenticate(options.Value, out var unauthorizedResult))
			{
				return unauthorizedResult!;
			}

			var evt = payload?.Event;
			if (evt is null || string.IsNullOrWhiteSpace(evt.Type))
			{
				return BadRequest("Missing FusionAuth event envelope.");
			}

			if (!RoleAffectingEvents.Contains(evt.Type))
			{
				// ACK so FusionAuth doesn't retry, but don't notify anyone.
				logger.LogDebug("Ignoring FusionAuth event of type {EventType}.", evt.Type);
				return Ok();
			}

			var userId = ResolveUserId(evt);
			if (userId is null || userId == Guid.Empty)
			{
				logger.LogWarning(
					"FusionAuth event {EventType} arrived without a resolvable user id; dropping.",
					evt.Type);
				return Ok();
			}

			var notification = new AuthEventNotification(
				Type: "RoleChanged",
				UserId: userId.Value,
				Source: evt.Type!,
				OccurredAt: timeProvider.GetUtcNow());

			await hubContext.Clients.User(userId.Value.ToString())
				.AuthEvent(notification);

			logger.LogInformation(
				"Forwarded FusionAuth {EventType} to AuthEventsHub for user {UserId}.",
				evt.Type, userId);

			return Ok();
		}

		private static Guid? ResolveUserId(FusionAuthEvent evt) =>
			evt.User?.Id is { } u && u != Guid.Empty ? u
			: evt.Registration?.UserId is { } r && r != Guid.Empty ? r
			: evt.UserId is { } x && x != Guid.Empty ? x
			: null;

		private bool TryAuthenticate(FusionAuthWebhookOptions opts, out IActionResult? failure)
		{
			failure = null;

			// No secret configured -> accept anything (intended for local dev only).
			if (string.IsNullOrEmpty(opts.Secret))
			{
				logger.LogWarning(
					"FusionAuth webhook received without a configured shared secret; accepting unconditionally. Configure 'FusionAuth:Webhook:Secret' for non-dev environments.");
				return true;
			}

			var header = Request.Headers.Authorization.ToString();
			const string expectedPrefix = "Bearer ";

			if (!header.StartsWith(expectedPrefix, StringComparison.Ordinal)
				|| !CryptographicEquals(header.AsSpan(expectedPrefix.Length), opts.Secret))
			{
				failure = Unauthorized();
				return false;
			}

			return true;
		}

		private static bool CryptographicEquals(ReadOnlySpan<char> a, string b)
		{
			// Constant-time equality on UTF-16 code units to avoid leaking the secret via timing.
			if (a.Length != b.Length)
			{
				return false;
			}

			var diff = 0;
			for (var i = 0; i < a.Length; i++)
			{
				diff |= a[i] ^ b[i];
			}
			return diff == 0;
		}
	}
}
