using OMS.Application.Interfaces.Persistation;
using OMS.Common.Models;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Handlers
{
    internal abstract class ApplicationRequestHandlerBase<TEntity>(
        IMediator mediator,
        IDbContext context)
        : BaseHandler<TEntity>(mediator, context)
        where TEntity : Entity, new()
    {
        //public override async Task<IResult<ServiceResponse<TEntity>>> HandleAsync(IDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        //{
        //    var result = await base.HandleAsync(request, cancellationToken);

        //    await Context.SaveChangesAsync(cancellationToken);

        //    return result;
        //}
    }
}
