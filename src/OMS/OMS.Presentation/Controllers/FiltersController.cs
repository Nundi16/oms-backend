using Microsoft.AspNetCore.Mvc;
using OMS.Common.Interfaces.Communication;

namespace OMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class FiltersController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
        {
            var optionsResult = await mediator.RequestAsync(cancellationToken);

            return optionsResult.Succeeded 
                ? Ok(optionsResult.Value)
                : BadRequest();
        }
    }
}
