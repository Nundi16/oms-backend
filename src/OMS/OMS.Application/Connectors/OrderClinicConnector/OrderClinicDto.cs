namespace OMS.Application.Connectors.OrderClinicConnector
{
	public sealed class OrderClinicDto : BaseConnectorDto
	{
        public override string Descriptor => nameof(OrderClinicDto);
		public string ClinicSpecificOrderName { get; init; } = String.Empty;
	}
}
