using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMS.Common.Interfaces.Communication;

namespace OMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class FiltersController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            //var optionsResult = await mediator.RequestAsync(cancellationToken);

            //return optionsResult.Succeeded 
            //    ? Ok(optionsResult.Value)
            //    : BadRequest();
        }
    }
}
