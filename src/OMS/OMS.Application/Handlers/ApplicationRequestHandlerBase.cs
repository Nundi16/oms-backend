using OMS.Application.Interfaces.Persistation;
using OMS.Application.Models;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Handlers
{
    internal class ApplicationRequestHandlerBase<TEntity>(IMediator mediator, IDbContext context) : RequestHandlerBase<TEntity>(mediator) 
        where TEntity : Entity, new()
    {
        public override Task<IResult<ServiceResponse<TEntity>>> HandleAsync(IDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            var result = base.HandleAsync(request, cancellationToken);

            context.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
