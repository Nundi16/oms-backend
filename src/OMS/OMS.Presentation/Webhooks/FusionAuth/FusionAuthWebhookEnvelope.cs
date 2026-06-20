using System.Text.Json.Serialization;

namespace OMS.Presentation.Webhooks.FusionAuth
{
	/// <summary>
	/// Minimal projection of the FusionAuth webhook payload. FusionAuth wraps every
	/// notification in an <c>event</c> object. We only deserialize the fields we care
	/// about (event type + the affected user id); everything else is intentionally
	/// dropped to keep the contract loose.
	/// </summary>
	public sealed class FusionAuthWebhookEnvelope
	{
		[JsonPropertyName("event")]
		public FusionAuthEvent? Event { get; set; }
	}

	public sealed class FusionAuthEvent
	{
		[JsonPropertyName("type")]
		public string? Type { get; set; }

		[JsonPropertyName("user")]
		public FusionAuthUser? User { get; set; }

		/// <summary>
		/// Present on <c>user.registration.*</c> events. Some FusionAuth event types
		/// (e.g. <c>user.delete</c>) only carry <see cref="User"/>; both shapes are
		/// supported by the controller.
		/// </summary>
		[JsonPropertyName("registration")]
		public FusionAuthRegistration? Registration { get; set; }

		/// <summary>
		/// Fallback for older / leaner event shapes that put the user id at the root
		/// instead of nested inside <c>user</c>.
		/// </summary>
		[JsonPropertyName("userId")]
		public Guid? UserId { get; set; }
	}

	public sealed class FusionAuthUser
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }
	}

	public sealed class FusionAuthRegistration
	{
		[JsonPropertyName("userId")]
		public Guid? UserId { get; set; }

		[JsonPropertyName("applicationId")]
		public Guid ApplicationId { get; set; }

		[JsonPropertyName("roles")]
		public IReadOnlyList<string>? Roles { get; set; }
	}
}
