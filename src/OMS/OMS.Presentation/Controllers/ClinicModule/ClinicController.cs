using Microsoft.AspNetCore.Mvc;
using OMS.Application.Modules.ClinicModule.Models;
using OMS.Domain.Modules.ClinicModule;

namespace OMS.Presentation.Controllers.ClinicModule
{
	[Route("api/[controller]")]
	public sealed class ClinicController : BaseCrudController<Clinic, ClinicDto>
	{
		public ClinicController(OMS.Common.Interfaces.Communication.IMediator mediator) : base(mediator)
		{
		}
	}
}
