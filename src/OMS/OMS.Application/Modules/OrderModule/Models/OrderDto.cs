namespace OMS.Application.Modules.OrderModule.Models
{
	public sealed record OrderDto(
		Guid? Id,
		string Name,
		string? Status,
		DateTime? DeliveryDate,
		string? DeliveryLocation,
		string? Notes,
		string? Product,
		DateTime? ScannedAt,
		string? PatientNo);
}
