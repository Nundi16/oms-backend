using AutoMapper;
using OMS.Application.Modules.ManufacturingModule.Models;
using OMS.Domain.Modules.ManufacturingModule;


namespace OMS.Application.Modules.ManufacturingModule
{
	internal sealed class ManufacturingMappingProfile : Profile
	{
		public ManufacturingMappingProfile()
		{
			CreateMap<SiteDto, Site>();
			CreateMap<Site, SiteDto>();
			CreateMap<MachineDto, Machine>();
			CreateMap<Machine, MachineDto>();
		}
	}
}
