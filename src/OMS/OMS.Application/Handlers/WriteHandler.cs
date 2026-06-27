using OMS.Application.Interfaces.Persistation;
using OMS.Common.Models;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers.Event;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Common.Interfaces.Connectors;
using OMS.Common.Interfaces.Entity;
using OMS.Common.Interfaces.Extensions;
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

            var extensionTasks = request.Extensions?.Select(extension =>
            {
                extension.AssignSourceId(entity.Id);

                return extension.DispatchCreationAsync(Mediator, cancellationToken);
            });

            var extensions = await Task.WhenAll(extensionTasks ?? []);

            if (request.PersistChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            return Result.Success(new ServiceResponse<TEntity>(entity, [], [])); //TODO: connectors
        }

        public async Task<IResult> HandleAsync(IDeletionDomainEvent<TEntity> @event, CancellationToken cancellationToken = default)
        {
            Context.Remove(@event.Entity);

            var connectorTasks = @event.Connectors?.Select(connector =>
                connector.DispatchDeletionAsync(Mediator, cancellationToken));

            await Task.WhenAll(connectorTasks ?? []);

            var extensionTasks = @event.Extensions?.Select(extension =>
                extension.DispatchDeletionAsync(Mediator, cancellationToken));

            await Task.WhenAll(extensionTasks ?? []);

            if (@event.PersistChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            return Result.Success();
        }

        public async Task<IResult<ServiceResponse<TEntity>>> HandleAsync(IModificationDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            TEntity entity;

            // Connectors and extensions follow upsert/PATCH semantics by design; the root aggregate must already exist.
            if (request.Entity is IConnector or IExtension)
            {
                entity = Context.Update(request.Entity);
            }
            else
            {
                var tracked = await Context.FindAsync<TEntity>(request.Entity.Id, cancellationToken);

                if (tracked is null)
                {
                    return Result.Failure<ServiceResponse<TEntity>>($"{typeof(TEntity).Name} with id '{request.Entity.Id}' was not found.");
                }

                Context.SetValues(tracked, request.Entity);
                entity = tracked;
            }

            var connectorTasks = request.Connectors?.Select(connector =>
            {
                connector.AssignSourceId(entity.Id);

                return connector.DispatchModificationAsync(Mediator, cancellationToken);
            });

            var connectors = await Task.WhenAll(connectorTasks ?? []);

            var extensionTasks = request.Extensions?.Select(extension =>
            {
                extension.AssignSourceId(entity.Id);

                return extension.DispatchModificationAsync(Mediator, cancellationToken);
            });

            var extensions = await Task.WhenAll(extensionTasks ?? []);

            if (request.PersistChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }

            return Result.Success(new ServiceResponse<TEntity>(entity, [], [])); //TODO: connectors, extensions
        }
    }
}
