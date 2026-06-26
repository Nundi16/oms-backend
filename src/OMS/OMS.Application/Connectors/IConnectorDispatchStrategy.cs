using OMS.Common.Interfaces.Communication;
using OMS.Domain.Interfaces.Connectors;

namespace OMS.Application.Connectors
{
    internal interface IConnectorDispatchStrategy
    {
        Type ConnectorType { get; }

        Task<IConnector?> DispatchCreationAsync(IConnector connector, IMediator mediator, CancellationToken cancellationToken);
        Task<IConnector?> DispatchModificationAsync(IConnector connector, IMediator mediator, CancellationToken cancellationToken);
        Task<IConnector?> DispatchDeletionAsync(IConnector connector, IMediator mediator, CancellationToken cancellationToken);
    }
}
