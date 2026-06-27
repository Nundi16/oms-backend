using OMS.Application.Interfaces.Persistation;
using OMS.Common.Models;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Common.Interfaces.Connectors;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Handlers
{
    internal abstract class BaseHandler<TEntity>(
        IMediator mediator,
        IDbContext context)
        : IRequestHandler<IDomainEvent<TEntity>, ServiceResponse<TEntity>>
        where TEntity : Entity, new()
    {
        protected readonly IMediator Mediator = mediator;
        protected readonly IDbContext Context = context;
        
        public virtual async Task<IResult<ServiceResponse<TEntity>>> HandleAsync(IDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            TEntity? entity;
            //Func<IConnector, CancellationToken, Task<IConnector?>> dispatchConnector;

            switch (request)
            {
                case ICreationDomainEvent<TEntity> creationDomainEvent:
                    entity = await Context.AddAsync(creationDomainEvent.Entity, cancellationToken);
                    foreach (var connector in request.Connectors ?? [])
                    {
                        connector.AssignSourceId(entity.Id);
                    }
                    break;
                case IModificationDomainEvent<TEntity> modificationDomainEvent:
                    entity = Context.Update(modificationDomainEvent.Entity);
                    break;
                case IDeletionDomainEvent<TEntity> deletionDomainEvent:
                    Context.Remove(deletionDomainEvent.Entity);
                    entity = default;
                    break;
                default:
                    throw new NotImplementedException();
            }

            var connectorTasks = (request.Connectors ?? [])
                .Select(async connector =>
                {
                    switch (request)
                    {
                        case ICreationDomainEvent<TEntity> creationDomainEvent:
                            return await Context.AddAsync(creationDomainEvent.Entity);
                        case IModificationDomainEvent<TEntity> modificationDomainEvent:
                            return Context.Update(modificationDomainEvent.Entity);
                        case IDeletionDomainEvent<TEntity> deletionDomainEvent:
                            Context.Remove(deletionDomainEvent.Entity);
                            return null;
                        default:
                            return null;
                    }
                })
                .ToArray();

            var processedConnectors = await Task.WhenAll(connectorTasks);
            var connectors = processedConnectors.Where(connector => connector is not null).OfType<IConnector>().ToArray();

            return Result.Success(new ServiceResponse<TEntity>(entity, connectors, []));
        }
    }
}
