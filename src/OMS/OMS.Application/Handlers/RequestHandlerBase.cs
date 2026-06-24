using OMS.Application.Extensions;
using OMS.Application.Helpers;
using OMS.Application.Interfaces.Communication;
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
    internal class RequestHandlerBase<TEntity>(IMediator mediator, IInfrastructureMediator infrastructureMediator) : IRequestHandler<IDomainEvent<TEntity>, ServiceResponse<TEntity>>
        where TEntity : Entity, new()
    {
        protected IMediator Mediator = mediator;
        protected IInfrastructureMediator InfrastructureMediator = infrastructureMediator;
        public virtual async Task<IResult<ServiceResponse<TEntity>>> HandleAsync(IDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            TEntity entity = null!;
            
            if (request is ICreationDomainEvent<TEntity> creationDomainEvent)
                entity = await InfrastructureMediator.HandleCreationAsync<ICreationDomainEvent<TEntity>, TEntity>(creationDomainEvent, cancellationToken);
            else if (request is IModificationDomainEvent<TEntity> modificationDomainEvent)
                entity = InfrastructureMediator.HandleModification<IModificationDomainEvent<TEntity>, TEntity>(modificationDomainEvent);
            else if (request is IDeletionDomainEvent<TEntity> deletionDomainEvent)
                entity = InfrastructureMediator.HandleDeletion<IDeletionDomainEvent<TEntity>, TEntity>(deletionDomainEvent);

            var tasks = request.Connectors
                .Select(connector => connector.ToDomainEvent([]))
                .Select(@event => DomainEventHelpers.InvokeHandler(@event, Mediator))
                .ToArray();

            var connectorHandlers = await Task.WhenAll(tasks);
            var connectors = connectorHandlers.Select(x => x.Value!.Entity as IConnector).ToArray();

            return Result.Success(new ServiceResponse<TEntity>(entity, connectors!));
        }
    }
}
