using AutoMapper;
using OMS.Application.Modules.OrderModule.Models;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application.Modules.OrderModule
{
	internal sealed class OrderMappingProfile : Profile
	{
		public OrderMappingProfile()
		{
			CreateMap<OrderDto, Order>()
				.ForMember(dest => dest.Id, opt => opt.Ignore()); // Id is managed by Entity base

			CreateMap<Order, OrderDto>()
				.ForMember(dest => dest.Connectors, opt => opt.Ignore()); // Connectors filled by reader pipeline
		}
	}
}
