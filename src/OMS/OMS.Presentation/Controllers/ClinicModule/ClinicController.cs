using Microsoft.AspNetCore.Mvc;
using OMS.Application.Modules.ClinicModule.Models;

namespace OMS.Presentation.Controllers.ClinicModule
{
	[Route("api/[controller]")]
	public sealed class ClinicController : BaseCrudController<ClinicDto>
	{
		public ClinicController(OMS.Common.Interfaces.Communication.IMediator mediator) : base(mediator)
		{
		}
	}
}
