using Microsoft.AspNetCore.Mvc;
using OMS.Application.Modules.ClinicModule.Models;
using OMS.Application.Modules.ManufacturingModule.Models;
using OMS.Domain.Modules.ManufacturingModule;

namespace OMS.Presentation.Controllers.ClinicModule
{
	[Route("api/[controller]")]
	public sealed class SiteController : BaseCrudController<Site, SiteDto>
	{
		public SiteController(OMS.Common.Interfaces.Communication.IMediator mediator) : base(mediator)
		{
		}
	}
}
