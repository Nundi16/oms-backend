using OMS.Application.Extensions;
using OMS.Application.Interfaces.Persistation;
using OMS.Application.Models;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers.Event;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Domain.Connectors;
using OMS.Domain.Interfaces.Connectors;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Handlers
{
    internal abstract class Handler<TEntity>(IMediator mediator, IDbContext context)
        : IRequestHandler<ICreationDomainEvent<TEntity>, ServiceResponse<TEntity>>,
        IRequestHandler<IModificationDomainEvent<TEntity>, ServiceResponse<TEntity>>,
        IEventHandler<IDeletionDomainEvent<TEntity>>
        where TEntity : Entity
    {
        protected readonly IMediator Mediator = mediator;
        protected readonly IDbContext Context = context;
        public async Task<IResult<ServiceResponse<TEntity>>> HandleAsync(ICreationDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            var entity = await Context.AddAsync(request.Entity, cancellationToken);

            IConnector[]? connectors = default;

            if (request.Connectors is { Length: not 0 })
            {
                var connectorTasks = request.Connectors.Select(connector =>
                {
                    connector.AssignSourceId(entity.Id);
                    return Mediator.RequestAsync(connector.ToCreationDomainEvent(), cancellationToken);
                }) ?? [];

                var results = await Task.WhenAll(connectorTasks);

                connectors = [.. 
                    results.Where(result => result.Succeeded && result.Value.Connectors is { Length: not 0 })
                        .SelectMany(result => result.Value!.Connectors!)
                    ];
            }

            await Context.SaveChangesAsync(cancellationToken);

            return Result.Success(new ServiceResponse<TEntity>(entity, connectors));
        }

        public async Task<IResult> HandleAsync(IDeletionDomainEvent<TEntity> @event, CancellationToken cancellationToken = default)
        {
            Context.Remove(@event.Entity);

            if(@event.Connectors is { Length: not 0 })
            {
                await Task.WhenAll(@event.Connectors.Select(connector => Mediator.EmitAsync(connector.ToCreationDomainEvent())));
            }

            await Context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<IResult<ServiceResponse<TEntity>>> HandleAsync(IModificationDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            var entity = Context.Update(request.Entity);

            IConnector[]? connectors = null;

            if (request.Connectors is { Length: not 0 })
            {
                var connectorTasks = request.Connectors.Select(connector =>
                {
                    connector.AssignSourceId(entity.Id);
                    return Mediator.RequestAsync(connector.ToModificationDomainEvent(), cancellationToken);
                }) ?? [];

                var results = await Task.WhenAll(connectorTasks);

                connectors = [..
                    results.Where(result => result.Succeeded && result.Value.Connectors is { Length: not 0 })
                    .SelectMany(result => result.Value!.Connectors!)
                    ];
            }

            await Context.SaveChangesAsync(cancellationToken);

            return Result.Success(new ServiceResponse<TEntity>(entity, connectors));
        }
    }
}
