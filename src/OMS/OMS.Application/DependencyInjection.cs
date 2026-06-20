using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Common.Interfaces;
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
			//TODO: Ez így buzis, ha saját assemblybe currentAssembly
			var domainAssemblyTypes = AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.GetName().Name?.StartsWith("OMS.Application") == true)
				.SelectMany(a => a.GetTypes());

			var dtoEntityPairs = domainAssemblyTypes
				.Where(t => !t.IsAbstract && t.IsClass && typeof(IDto).IsAssignableFrom(t))
				.Select(t => new
				{
					DtoType = t,
					DtoInterface = t.GetInterfaces().FirstOrDefault(i =>
						i.IsGenericType &&
						i.GetGenericTypeDefinition() == typeof(IDto<>))
				})
				.Where(x => x.DtoInterface is not null)
				.Select(x => (DtoType: x.DtoType, EntityType: x.DtoInterface!.GetGenericArguments()[0]))
				.ToList();

			foreach (var (dtoType, entityType) in dtoEntityPairs)
			{
				services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(
					typeof(BaseCreateRequestDto<,>).MakeGenericType(entityType, dtoType),
					typeof(BaseResponseDto<>).MakeGenericType(entityType)),
					typeof(BaseCreateRequestDtoHandler<,>).MakeGenericType(entityType, dtoType));
				services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(
					typeof(BaseUpdateRequestDto<,>).MakeGenericType(entityType, dtoType),
					typeof(BaseResponseDto<>).MakeGenericType(entityType)),
					typeof(BaseUpdateRequestDtoHandler<,>).MakeGenericType(entityType, dtoType));
				services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(
					typeof(BaseGetByIdRequestDto<,>).MakeGenericType(entityType, dtoType),
					typeof(BaseResponseDto<>).MakeGenericType(entityType)),
					typeof(BaseGetByIdRequestDtoHandler<,>).MakeGenericType(entityType, dtoType));
				services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(
					typeof(BaseGetAllRequestDto<,>).MakeGenericType(entityType, dtoType),
					typeof(BaseListResponseDto<>).MakeGenericType(dtoType)),
					typeof(BaseGetAllRequestDtoHandler<,>).MakeGenericType(entityType, dtoType));
				services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(
					typeof(BaseDeleteRequestDto<,>).MakeGenericType(entityType, dtoType),
					typeof(BaseDeleteResponseDto)),
					typeof(BaseDeleteRequestDtoHandler<,>).MakeGenericType(entityType, dtoType));
			}

			return services;
        }
    }
}
