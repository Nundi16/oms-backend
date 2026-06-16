using OMS.Application.Interfaces.Communication;
using OMS.Application.Modules.OrderModule.Events;
using OMS.Application.Modules.OrderModule.Models;
using OMS.Common;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application.Modules.OrderModule
{
    internal class OrderCreationRequestHandler(IInfrastructureMediator mediator) : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        public async Task<IResult<CreateOrderResponse>> HandleAsync(CreateOrderRequest @event, CancellationToken cancellationToken = default)
        {
            var entity = new Order { Name = @event.Name };
            var order = await mediator.HandleCreationAsync<OrderCreationEvent, Order>(new OrderCreationEvent(entity), cancellationToken);

            return Result.Success(new CreateOrderResponse(order.Id, order.Name));
        }
    }
}
