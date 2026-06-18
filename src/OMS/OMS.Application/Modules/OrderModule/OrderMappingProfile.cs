using AutoMapper;
using OMS.Application.Modules.OrderModule.Models;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application.Modules.OrderModule
{
	internal sealed class OrderMappingProfile : Profile
	{
		public OrderMappingProfile()
		{
			CreateMap<OrderDto, Order>();
			CreateMap<Order, OrderDto>();
		}
	}
}
