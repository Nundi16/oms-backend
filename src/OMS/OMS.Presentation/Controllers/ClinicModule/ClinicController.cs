using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMS.Common.Models;
using OMS.Common.Interfaces.Communication;
using OMS.Domain;
using OMS.Domain.Modules.ClinicModule;

namespace OMS.Presentation.Controllers.ClinicModule
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Clinic)]
    public sealed class ClinicController(IMediator mediator) : CrudControllerBase<Clinic, ServiceResponse<Clinic>>(mediator)
    {
    }
}
