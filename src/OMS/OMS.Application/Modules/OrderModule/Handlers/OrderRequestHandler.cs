using OMS.Application.Handlers;
using OMS.Application.Interfaces.Persistation;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application.Modules.OrderModule.Handlers
{
    internal sealed class OrderRequestHandler(IMediator mediator, IDbContext context) : Handler<Order>(mediator, context);
}
