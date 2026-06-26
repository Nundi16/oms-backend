using OMS.Domain.Interfaces.Connectors;

namespace OMS.Application.Connectors
{
    public interface IConnectorEventDispatcher
    {
        Task<IConnector?> DispatchCreationAsync(IConnector connector, CancellationToken cancellationToken = default);
        Task<IConnector?> DispatchModificationAsync(IConnector connector, CancellationToken cancellationToken = default);
        Task<IConnector?> DispatchDeletionAsync(IConnector connector, CancellationToken cancellationToken = default);
    }
}
