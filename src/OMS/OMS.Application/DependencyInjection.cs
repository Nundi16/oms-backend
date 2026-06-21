using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Common.Interfaces;
using OMS.Application.Communication.Handlers;
using OMS.Application.Communication.Requests;
using OMS.Application.Communication.Responses;
using OMS.Common.Communication;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using System.Reflection;

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
			services.RegisterFilterableService();

            return services;
        }


		private static IServiceCollection RegisterFilterableService(this IServiceCollection services)
		{
            var domainAssemblyTypes = AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.GetName().Name?.StartsWith("OMS.Domain") == true)
				.SelectMany(a => a.GetTypes());

            var entityTypes = domainAssemblyTypes
                .Where(t => !t.IsAbstract && t.IsClass && typeof(OMS.Common.Abstractions.Entity.Entity).IsAssignableFrom(t))
                .ToList();

			foreach (var entityType in entityTypes)
			{
                services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(
					typeof(BaseFilterRequestDto<>).MakeGenericType(entityType),
					typeof(BaseFilterResponseDto)),
					typeof(GetAllFilterHandler<>).MakeGenericType(entityType));
            }

			return services;
        }

        private static IServiceCollection RegisterCrudDtoHandlers(this IServiceCollection services)
        {
			var dtoEntityPairs = Assembly.GetCallingAssembly().GetTypes()
				.Where(t => !t.IsAbstract && t.IsClass && typeof(IDto).IsAssignableFrom(t))
                .Select(t => (DtoType: t, EntytyType: t.GetInterfaces().FirstOrDefault(i =>
					i.IsGenericType == true && i.GetGenericTypeDefinition() == typeof(IDto<>))!.GetGenericArguments()[0]
				)).ToList();

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
