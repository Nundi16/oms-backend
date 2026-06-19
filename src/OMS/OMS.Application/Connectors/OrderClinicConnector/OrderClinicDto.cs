namespace OMS.Application.Connectors.OrderClinicConnector
{
	/// <summary>
	/// DTO representing the connector between Order and Clinic entities.
	/// Carries the clinic-specific order name in addition to the base connector fields.
	/// </summary>
	public sealed class OrderClinicDto : BaseConnectorDto
	{
        public override string Descriptor => nameof(OrderClinicDto);
		/// <summary>
		/// A clinic-specific name or identifier for this order, if applicable.
		/// </summary>
		public string? ClinicSpecificOrderName { get; init; }
	}
}
