using OMS.Application.Connectors;
using OMS.Application.Connectors.Abstractions;

namespace OMS.Application.Modules.OrderModule.Models
{
	/// <summary>
	/// Data transfer object for Order entity. Implements IConnectorsCarrier to participate
	/// in the connector read/write pipeline.
	/// </summary>
	public sealed class OrderDto : IConnectorsCarrier
	{
		public Guid? Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string? Status { get; init; }
		public DateTime? DeliveryDate { get; init; }
		public string? DeliveryLocation { get; init; }
		public string? Notes { get; init; }
		public string? Product { get; init; }
		public DateTime? ScannedAt { get; init; }
		public string? PatientNo { get; init; }

		/// <summary>
		/// Polymorphic collection of connector DTOs associated with this order.
		/// Null = no connector data; empty list = explicit removal of all connectors.
		/// </summary>
		public IReadOnlyList<BaseConnectorDto>? Connectors { get; set; }
	}
}

