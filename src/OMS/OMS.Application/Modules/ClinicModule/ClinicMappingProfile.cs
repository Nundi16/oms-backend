using AutoMapper;
using OMS.Application.Modules.ClinicModule.Models;
using OMS.Domain.Modules.ClinicModule;

namespace OMS.Application.Modules.ClinicModule
{
	internal sealed class ClinicMappingProfile : Profile
	{
		public ClinicMappingProfile()
		{
			CreateMap<ClinicDto, Clinic>();
			CreateMap<Clinic, ClinicDto>();
		}
	}
}
