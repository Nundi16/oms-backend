using Microsoft.AspNetCore.Mvc;
using OMS.Application.Modules.OrderModule.Models;
using OMS.Common.Interfaces.Communication;
using OMS.Presentation.Controllers.OrderModule.Contracts;

namespace OMS.Presentation.Controllers.OrderModule
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class OrderController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderRequest request, CancellationToken cancellationToken = default) 
        {
            var result = await mediator.RequestAsync<CreateOrderRequestDto, CreateOrderResponseDto>(request.Order, cancellationToken);
            return Ok(result);
        }
    }
}
