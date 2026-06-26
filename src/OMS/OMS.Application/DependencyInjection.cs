using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Connectors;
using OMS.Common.Communication;

namespace OMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.RegisterHandlersFromCurrentAssembly();
            services.RegisterConnectorDispatch();

            return services;
        }
    }
}
