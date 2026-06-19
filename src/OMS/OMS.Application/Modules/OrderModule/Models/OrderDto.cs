using OMS.Application.Common.Interfaces;
using OMS.Application.Connectors;
using OMS.Application.Connectors.Abstractions;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application.Modules.OrderModule.Models
{
	/// <summary>
	/// Data transfer object for Order entity. Implements IConnectorsCarrier to participate
	/// in the connector read/write pipeline.
	/// </summary>
	public sealed class OrderDto(Guid? id, string name, string? status, DateTime? deliveryDate) : IConnectorsCarrier, IDto<Order>
	{
		public Guid? Id { get; set; } = id;
		public string Name { get; set; } = name ?? string.Empty;
		public string? Status { get; set; } = status;
		public DateTime? DeliveryDate { get; set; } = deliveryDate;
		public string? DeliveryLocation { get; set; }
		public string? Notes { get; set; }
		public string? Product { get; set; }
		public DateTime? ScannedAt { get; set; }
		public string? PatientNo { get; set; }

		/// <summary>
		/// Polymorphic collection of connector DTOs associated with this order.
		/// Null = no connector data; empty list = explicit removal of all connectors.
		/// </summary>
		public IReadOnlyList<BaseConnectorDto>? Connectors { get; set; }
	}
}

