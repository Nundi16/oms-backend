using Microsoft.Extensions.DependencyInjection;
using OMS.Common.Communication;

namespace OMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.RegisterHandlersFromCurrentAssembly();

            return services;
        }
    }
}
