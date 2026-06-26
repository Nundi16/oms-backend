using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Connectors;

namespace OMS.Application.Connectors
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterConnectorDispatch(this IServiceCollection services)
        {
            services.AddScoped<IConnectorEventDispatcher, ConnectorEventDispatcher>();

            foreach (var connectorType in GetConcreteConnectorTypes())
            {
                var strategyType = typeof(ConnectorDispatchStrategy<>).MakeGenericType(connectorType);
                services.AddSingleton(typeof(IConnectorDispatchStrategy), strategyType);
            }

            return services;
        }

        private static IEnumerable<TypeInfo> GetConcreteConnectorTypes() =>
            typeof(IConnector).Assembly.DefinedTypes.Where(IsConcreteConnector);

        private static bool IsConcreteConnector(TypeInfo type) =>
            type is { IsAbstract: false, IsInterface: false, IsGenericTypeDefinition: false, ContainsGenericParameters: false }
            && typeof(IConnector).IsAssignableFrom(type)
            && typeof(Entity).IsAssignableFrom(type)
            && type.GetConstructor(Type.EmptyTypes) is not null;
    }
}
