using OMS.Application.Common.Interfaces;
using OMS.Domain.Modules.ClinicModule;

namespace OMS.Application.Modules.ClinicModule.Models
{
	public sealed class ClinicDto : IDto<Clinic>
	{
		public Guid? Id { get; set; }
		public string Name { get; set; }
	}
}
