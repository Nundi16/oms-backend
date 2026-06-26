using OMS.Application.Models;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Abstractions.Events;
using OMS.Domain.Interfaces.Connectors;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Connectors
{
    internal sealed class ConnectorDispatchStrategy<TConnector> : IConnectorDispatchStrategy
        where TConnector : Entity, IConnector, new()
    {
        public Type ConnectorType => typeof(TConnector);

        public Task<IConnector?> DispatchCreationAsync(IConnector connector, IMediator mediator, CancellationToken cancellationToken) =>
            SendAsync(new CreationDomainEvent<TConnector>((TConnector)connector, []), mediator, cancellationToken);

        public Task<IConnector?> DispatchModificationAsync(IConnector connector, IMediator mediator, CancellationToken cancellationToken) =>
            SendAsync(new ModificationDomainEvent<TConnector>((TConnector)connector, []), mediator, cancellationToken);

        public Task<IConnector?> DispatchDeletionAsync(IConnector connector, IMediator mediator, CancellationToken cancellationToken) =>
            SendAsync(new DeletionDomainEvent<TConnector>((TConnector)connector, []), mediator, cancellationToken);

        private static async Task<IConnector?> SendAsync(IDomainEvent<TConnector> @event, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.RequestAsync<IDomainEvent<TConnector>, ServiceResponse<TConnector>>(@event, cancellationToken);
            return result.Value?.Entity;
        }
    }
}
