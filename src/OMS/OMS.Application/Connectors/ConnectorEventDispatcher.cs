using OMS.Common.Interfaces.Communication;
using OMS.Domain.Interfaces.Connectors;

namespace OMS.Application.Connectors
{
    internal sealed class ConnectorEventDispatcher(
        IMediator mediator,
        IEnumerable<IConnectorDispatchStrategy> strategies) : IConnectorEventDispatcher
    {
        private readonly IMediator _mediator = mediator;
        private readonly IReadOnlyDictionary<Type, IConnectorDispatchStrategy> _strategies =
            strategies.ToDictionary(strategy => strategy.ConnectorType);

        public Task<IConnector?> DispatchCreationAsync(IConnector connector, CancellationToken cancellationToken = default) =>
            Resolve(connector).DispatchCreationAsync(connector, _mediator, cancellationToken);

        public Task<IConnector?> DispatchModificationAsync(IConnector connector, CancellationToken cancellationToken = default) =>
            Resolve(connector).DispatchModificationAsync(connector, _mediator, cancellationToken);

        public Task<IConnector?> DispatchDeletionAsync(IConnector connector, CancellationToken cancellationToken = default) =>
            Resolve(connector).DispatchDeletionAsync(connector, _mediator, cancellationToken);

        private IConnectorDispatchStrategy Resolve(IConnector connector) =>
            _strategies.TryGetValue(connector.GetType(), out var strategy)
                ? strategy
                : throw new InvalidOperationException(
                    $"No dispatch strategy registered for connector type '{connector.GetType().FullName}'.");
    }
}
