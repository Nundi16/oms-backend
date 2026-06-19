using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMS.Application.Modules.OrderModule.Models;
using OMS.Common.Interfaces.Communication;
using OMS.Domain;
using OMS.Presentation.Controllers.OrderModule.Contracts;
using OMS.Presentation.Models.Pagination;

namespace OMS.Presentation.Controllers.OrderModule
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Order)]
    public sealed class OrdersController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] PaginationRequest pagination, 
            //[FromQuery] FilterRequest filter, 
            CancellationToken cancellationToken = default)
        {
            return Ok();
        }

        [HttpGet]
        [Route("{id:guid:required}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderApiRequest request, CancellationToken cancellationToken = default) 
        {
            var result = await mediator.RequestAsync<CreateOrderRequest, CreateOrderResponse>(new CreateOrderRequest(request.Name), cancellationToken);
            return Ok(result);
        }
    }
}
