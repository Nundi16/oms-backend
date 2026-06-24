using OMS.Application.Extensions;
using OMS.Application.Helpers;
using OMS.Application.Models;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Handlers
{
    internal class RequestHandlerBase<TEntity>(IMediator mediator) : IRequestHandler<IDomainEvent<TEntity>, ServiceResponse<TEntity>>
        where TEntity : Entity, new()
    {
        protected IMediator Mediator = mediator;
        public virtual Task<IResult<ServiceResponse<TEntity>>> HandleAsync(IDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            var tasks = request.Connectors
                .Select(connector => connector.ToDomainEvent([]))
                .Select(@event => DomainEventHelpers.InvokeHandler(@event, Mediator))
                .ToArray();

            var asd = Task.WhenAll(tasks);

            //return Result.Success()T
        }
    }
}
