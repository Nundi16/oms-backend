namespace OMS.Application.Connectors.Abstractions
{
	/// <summary>
	/// Marker interface for DTOs that carry a polymorphic collection of connector DTOs.
	/// Implementing this interface signals to the generic CRUD pipeline that the DTO
	/// should participate in connector read/write operations.
	/// </summary>
	public interface IConnectorsCarrier
	{
		/// <summary>
		/// Gets or sets the collection of connector DTOs associated with this entity.
		/// Null indicates no connector data; an empty collection indicates explicit removal of all connectors.
		/// </summary>
		IReadOnlyList<BaseConnectorDto>? Connectors { get; set; }
	}
}
