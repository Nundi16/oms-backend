using OMS.Application.Handlers;
using OMS.Application.Interfaces.Persistation;
using OMS.Application.Modules.OrderModule.Authorization;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Modules.OrderClinicConnector;

namespace OMS.Application.Connectors.OrderClinicConnector.Handlers
{
    internal sealed class OrderClinicRequestHandler(IMediator mediator,IDbContext context, IOrderAuthorizationGuard guard) 
        : Handler<OrderClinic, IOrderAuthorizationGuard>(mediator, context, guard);
}
