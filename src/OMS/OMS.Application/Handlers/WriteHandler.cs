using OMS.Application.Interfaces.Persistation;
using OMS.Common.Models;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers.Event;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Common.Interfaces.Entity;
using OMS.Domain.Abstractions.Events;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Handlers
{
    internal class WriteHandler<TEntity>(IMediator mediator, IDbContext context)
        : IRequestHandler<ICreationDomainEvent<TEntity>, ServiceResponse<TEntity>>,
        IRequestHandler<IModificationDomainEvent<TEntity>, ServiceResponse<TEntity>>,
        IEventHandler<IDeletionDomainEvent<TEntity>>
        where TEntity : Entity, new()
    {
        protected readonly IMediator Mediator = mediator;
        protected readonly IDbContext Context = context;
        public async Task<IResult<ServiceResponse<TEntity>>> HandleAsync(ICreationDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            var entity = await Context.AddAsync(request.Entity, cancellationToken);

            var connectorTasks = request.Connectors?.Select(connector =>
            {
                connector.AssignSourceId(entity.Id);
                // map to domain event
                // Call Mediator extensions - eg.: Mediator.RequestAsnyc(domainEvent)

                return connector.DispatchCreationAsync(Mediator, cancellationToken);
            });

            var connectors = await Task.WhenAll(connectorTasks ?? []);

            if (request.PersistChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            return Result.Success(new ServiceResponse<TEntity>(entity, [])); //TODO: connectors
        }

        public async Task<IResult> HandleAsync(IDeletionDomainEvent<TEntity> @event, CancellationToken cancellationToken = default)
        {
            Context.Remove(@event.Entity);

            if(@event.Connectors is { Length: not 0 })
            {
                foreach (var connector in @event.Connectors)
                {
                    Context.Remove(connector);
                }
            }

            await Context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<IResult<ServiceResponse<TEntity>>> HandleAsync(IModificationDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            var entity = Context.Update(request.Entity);

            var connectors = request.Connectors?.Select(connector =>
            {
                connector.AssignSourceId(entity.Id);
                return Context.Update(connector);
            }).ToArray() ?? [];

            await Context.SaveChangesAsync(cancellationToken);

            return Result.Success(new ServiceResponse<TEntity>(entity, connectors));
        }
    }
}
