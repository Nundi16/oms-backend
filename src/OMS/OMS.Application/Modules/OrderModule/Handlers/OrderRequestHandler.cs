using OMS.Application.Handlers;
using OMS.Application.Interfaces.Persistation;
using OMS.Application.Modules.OrderModule.Authorization;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application.Modules.OrderModule.Handlers
{
    internal sealed class OrderRequestHandler(IMediator mediator, IDbContext context, IOrderAuthorizationGuard guard)
        : Handler<Order, IOrderAuthorizationGuard>(mediator, context, guard);
}
