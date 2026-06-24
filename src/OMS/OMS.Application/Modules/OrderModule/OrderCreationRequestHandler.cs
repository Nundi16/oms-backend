using OMS.Application.Interfaces.Communication;
using OMS.Application.Interfaces.Persistation;
using OMS.Application.Modules.OrderModule.Events;
using OMS.Application.Modules.OrderModule.Models;
using OMS.Common;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application.Modules.OrderModule
{
    internal class OrderCreationRequestHandler(IInfrastructureMediator mediator, IDbContext context) : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        public async Task<IResult<CreateOrderResponse>> HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            var entity = new Order { Name = request.Name };
            var order = await mediator.HandleCreationAsync<OrderCreationEvent, Order>(new OrderCreationEvent(entity), cancellationToken);

            // iterate connectors
            // let handler handle
            // save

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateOrderResponse(order.Id, order.Name));
        }
    }
}
