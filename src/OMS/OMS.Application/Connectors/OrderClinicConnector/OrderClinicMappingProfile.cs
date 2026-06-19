using AutoMapper;
using OMS.Domain.Connectors.OrderClinicConnector;

namespace OMS.Application.Connectors.OrderClinicConnector
{
	/// <summary>
	/// AutoMapper profile for mapping between OrderClinicDto and OrderClinic entity.
	/// Navigation properties (Parent/Dependant) are ignored; only FK properties (ParentId/DependantId)
	/// and connector-specific fields are mapped.
	/// </summary>
	internal sealed class OrderClinicMappingProfile : Profile
	{
		public OrderClinicMappingProfile()
		{
			CreateMap<OrderClinicDto, OrderClinic>()
				.ForMember(dest => dest.Parent, opt => opt.Ignore())
				.ForMember(dest => dest.Dependant, opt => opt.Ignore());

			CreateMap<OrderClinic, OrderClinicDto>();
		}
	}
}
