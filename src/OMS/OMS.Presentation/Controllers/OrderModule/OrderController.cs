using Microsoft.AspNetCore.Mvc;
using OMS.Application.Modules.OrderModule.Models;

namespace OMS.Presentation.Controllers.OrderModule
{
    [Route("api/[controller]")]
    public sealed class OrderController : BaseCrudController<OrderDto>
    {
        public OrderController(OMS.Common.Interfaces.Communication.IMediator mediator) : base(mediator)
        {
        }
    }
}
