using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Connectors.Pipeline
{
	/// <summary>
	/// Mutable context event fanned out via <c>IMediator.FanOutSequentialAsync</c> when
	/// loading parent entities. Connector-specific authorized event handlers query their
	/// connector tables for the requested <see cref="ParentIds"/> and append their DTOs
	/// to <see cref="Accumulator"/>, keyed by parent id. The CRUD handler then attaches
	/// the accumulated connectors to the corresponding parent DTOs via
	/// <c>IConnectorsCarrier.Connectors</c>.
	/// </summary>
	/// <typeparam name="TParent">The parent entity type whose connectors are being loaded.</typeparam>
	public sealed class ConnectorLoadContext<TParent> where TParent : Entity
	{
		public ConnectorLoadContext(IReadOnlyCollection<Guid> parentIds)
		{
			ArgumentNullException.ThrowIfNull(parentIds);
			ParentIds = parentIds;
			Accumulator = new Dictionary<Guid, List<BaseConnectorDto>>();
		}

		/// <summary>
		/// The parent entity ids whose connectors must be loaded.
		/// </summary>
		public IReadOnlyCollection<Guid> ParentIds { get; }

		/// <summary>
		/// Mutable accumulator that authorized handlers append to. Keyed by parent id;
		/// each value is the running list of connectors discovered so far.
		/// </summary>
		public Dictionary<Guid, List<BaseConnectorDto>> Accumulator { get; }
	}
}
