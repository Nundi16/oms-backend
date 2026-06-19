using Microsoft.AspNetCore.Mvc;
using OMS.Application.Modules.OrderModule.Models;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Presentation.Controllers.OrderModule
{
    [Route("api/[controller]")]
    public sealed class OrderController : BaseCrudController<Order, OrderDto>
    {
        public OrderController(OMS.Common.Interfaces.Communication.IMediator mediator) : base(mediator)
        {
        }
    }
}
