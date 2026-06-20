namespace OMS.Presentation.Webhooks.FusionAuth
{
	/// <summary>
	/// Bound from the <c>FusionAuth:Webhook</c> configuration section.
	/// </summary>
	public sealed class FusionAuthWebhookOptions
	{
		public const string SectionName = "FusionAuth:Webhook";

		/// <summary>
		/// Shared secret that FusionAuth must send in the <c>Authorization: Bearer ...</c>
		/// header (configured under "HTTP Headers" on the FusionAuth Webhook screen).
		/// Empty / null means "do not validate" — should only be used in local dev.
		/// </summary>
		public string? Secret { get; set; }
	}
}
