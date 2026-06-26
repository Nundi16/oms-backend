using OMS.Application.Handlers;
using OMS.Application.Interfaces.Persistation;
using OMS.Application.Models;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Connectors.OrderClinicConnector;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Connectors.OrderClinicConnector.Handlers
{
    internal sealed class OrderClinicRequestHandler(
        IMediator mediator,
        IRepository<OrderClinic> repository,
        IConnectorEventDispatcher connectorEventDispatcher)
        : RequestHandlerBase<OrderClinic>(mediator, repository, connectorEventDispatcher);

        
}
