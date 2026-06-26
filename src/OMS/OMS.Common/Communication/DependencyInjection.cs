using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OMS.Common.Communication.Authorization.Guards;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Common.Interfaces.Communication.Handlers;
using OMS.Common.Interfaces.Communication.Handlers.Event;
using OMS.Common.Interfaces.Communication.Handlers.Request;

namespace OMS.Common.Communication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddScoped<IMediator, AuthorizingMediator>();
            services.AddScoped<IMediatorAuthorizationGuard, MediatorAuthorizationGuard>();

            return services;
        }

        public static IServiceCollection RegisterHandlersFromCurrentAssembly(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();

            if (assembly is not null)
            {
                var handlerTypes = GetHandlerImplementationTypesFromAssembly(assembly);

                var descriptors = CreateServiceDescriptors(handlerTypes);

                services.Add(descriptors);
            }

            return services;
        }

        private static IEnumerable<ServiceDescriptor> CreateServiceDescriptors(IEnumerable<TypeInfo> typeInfos) =>
            typeInfos.SelectMany(typeInfo =>
            {
                var lifetime = GetLifetimeForHandler(typeInfo);
                var serviceInterfaces = GetUnderlyingInterfacesForHandler(typeInfo);
                return serviceInterfaces.Select(interfaceType => new ServiceDescriptor(interfaceType, typeInfo, lifetime));
            });

        private static ServiceLifetime GetLifetimeForHandler(Type type) =>
            typeof(IScopedHandler).IsAssignableFrom(type)
                    ? ServiceLifetime.Scoped
                    : ServiceLifetime.Transient;

        private static IEnumerable<TypeInfo> GetHandlerImplementationTypesFromAssembly(Assembly assembly) =>
            assembly.DefinedTypes.Where(type => type is { IsAbstract: false, IsInterface: false } && typeof(IHandler).IsAssignableFrom(type));

        private static IEnumerable<Type> GetUnderlyingInterfacesForHandler(TypeInfo typeInfo) =>
            typeInfo.GetInterfaces().Where(type => type.IsGenericType && type.Implements(typeof(IRequestHandler<,>), typeof(IEventHandler<>)));

        private static bool Implements(this Type type, params Type[] interfaceTypes) =>
            interfaceTypes.Any(interfaceType => type.GetGenericTypeDefinition() == interfaceType);
    }
}
