using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Communication.Handlers;
using OMS.Application.Communication.Requests;
using OMS.Application.Communication.Responses;
using OMS.Application.Modules.ClinicModule.Models;
using OMS.Application.Modules.OrderModule.Models;
using OMS.Common.Communication;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Domain.Modules.ClinicModule;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.RegisterCrudDtoHandlers();

            // Register specific handlers from the assembly
            // These will override the generic handlers (from Infrastructure) for specific entity types
            services.RegisterHandlersFromCurrentAssembly();

            return services;
        }

        private static IServiceCollection RegisterCrudDtoHandlers(this IServiceCollection services)
        {
            // Order
            services.AddScoped<IRequestHandler<BaseCreateRequestDto<OrderDto>, BaseResponseDto<OrderDto>>, BaseCreateRequestDtoHandler<Order, OrderDto>>();
            services.AddScoped<IRequestHandler<BaseGetByIdRequestDto<OrderDto>, BaseResponseDto<OrderDto>>, BaseGetByIdRequestDtoHandler<Order, OrderDto>>();
            services.AddScoped<IRequestHandler<BaseGetAllRequestDto<OrderDto>, BaseListResponseDto<OrderDto>>, BaseGetAllRequestDtoHandler<Order, OrderDto>>();
            services.AddScoped<IRequestHandler<BaseUpdateRequestDto<OrderDto>, BaseResponseDto<OrderDto>>, BaseUpdateRequestDtoHandler<Order, OrderDto>>();
            services.AddScoped<IRequestHandler<BaseDeleteRequestDto<OrderDto>, BaseDeleteResponseDto>, BaseDeleteRequestDtoHandler<Order, OrderDto>>();

            // Clinic
            services.AddScoped<IRequestHandler<BaseCreateRequestDto<ClinicDto>, BaseResponseDto<ClinicDto>>, BaseCreateRequestDtoHandler<Clinic, ClinicDto>>();
            services.AddScoped<IRequestHandler<BaseGetByIdRequestDto<ClinicDto>, BaseResponseDto<ClinicDto>>, BaseGetByIdRequestDtoHandler<Clinic, ClinicDto>>();
            services.AddScoped<IRequestHandler<BaseGetAllRequestDto<ClinicDto>, BaseListResponseDto<ClinicDto>>, BaseGetAllRequestDtoHandler<Clinic, ClinicDto>>();
            services.AddScoped<IRequestHandler<BaseUpdateRequestDto<ClinicDto>, BaseResponseDto<ClinicDto>>, BaseUpdateRequestDtoHandler<Clinic, ClinicDto>>();
            services.AddScoped<IRequestHandler<BaseDeleteRequestDto<ClinicDto>, BaseDeleteResponseDto>, BaseDeleteRequestDtoHandler<Clinic, ClinicDto>>();

            return services;
        }
    }
}
