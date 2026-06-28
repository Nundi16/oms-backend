using OMS.Application.Handlers;
using OMS.Application.Interfaces.Persistation;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Modules.OrderClinicConnector;

namespace OMS.Application.Connectors.OrderClinicConnector.Handlers
{
    internal sealed class OrderClinicRequestHandler(IMediator mediator,IDbContext context) : Handler<OrderClinic>(mediator, context);
}
