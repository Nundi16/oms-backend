using OMS.Application.Connectors;
using OMS.Application.Handlers;
using OMS.Application.Interfaces.Persistation;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application.Modules.OrderModule.Handlers
{
    internal sealed class OrderRequestHandler(
        IMediator mediator,
        IRepository<Order> repository,
        IUnitOfWork unitOfWork,
        IConnectorEventDispatcher connectorEventDispatcher)
        : ApplicationRequestHandlerBase<Order>(mediator, repository, unitOfWork, connectorEventDispatcher);
}
