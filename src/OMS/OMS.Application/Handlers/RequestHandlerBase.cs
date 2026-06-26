using OMS.Application.Connectors;
using OMS.Application.Interfaces.Persistation;
using OMS.Application.Models;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Domain.Interfaces.Connectors;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Handlers
{
    internal abstract class RequestHandlerBase<TEntity>(
        IMediator mediator,
        IRepository<TEntity> repository,
        IConnectorEventDispatcher connectorEventDispatcher)
        : IRequestHandler<IDomainEvent<TEntity>, ServiceResponse<TEntity>>
        where TEntity : Entity, new()
    {
        protected IMediator Mediator = mediator;
        protected IRepository<TEntity> Repository = repository;
        protected IConnectorEventDispatcher ConnectorEventDispatcher = connectorEventDispatcher;

        public virtual async Task<IResult<ServiceResponse<TEntity>>> HandleAsync(IDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            TEntity entity;
            Func<IConnector, CancellationToken, Task<IConnector?>> dispatchConnector;

            switch (request)
            {
                case ICreationDomainEvent<TEntity> creationDomainEvent:
                    entity = await Repository.AddAsync(creationDomainEvent.Entity, cancellationToken);
                    foreach (var connector in request.Connectors ?? [])
                    {
                        connector.AssignSourceId(entity.Id);
                    }
                    dispatchConnector = ConnectorEventDispatcher.DispatchCreationAsync;
                    break;
                case IModificationDomainEvent<TEntity> modificationDomainEvent:
                    entity = Repository.Update(modificationDomainEvent.Entity);
                    dispatchConnector = ConnectorEventDispatcher.DispatchModificationAsync;
                    break;
                case IDeletionDomainEvent<TEntity> deletionDomainEvent:
                    entity = Repository.Remove(deletionDomainEvent.Entity);
                    dispatchConnector = ConnectorEventDispatcher.DispatchDeletionAsync;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var connectorTasks = (request.Connectors ?? [])
                .Select(connector => dispatchConnector(connector, cancellationToken))
                .ToArray();

            var processedConnectors = await Task.WhenAll(connectorTasks);
            var connectors = processedConnectors.OfType<IConnector>().ToArray();

            return Result.Success(new ServiceResponse<TEntity>(entity, connectors));
        }
    }
}
