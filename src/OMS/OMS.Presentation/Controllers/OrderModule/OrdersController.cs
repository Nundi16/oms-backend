using Microsoft.AspNetCore.Mvc;
using OMS.Application.Models;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Presentation.Controllers.OrderModule
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = Roles.Order)]
    public sealed class OrdersController(IMediator mediator) : CrudControllerBase<Order, ServiceResponse<Order>>(mediator);
}
