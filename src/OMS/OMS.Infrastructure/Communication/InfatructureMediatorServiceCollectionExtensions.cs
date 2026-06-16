using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Interfaces.Communication;

namespace OMS.Infrastructure.Communication
{
    public static class InfatructureMediatorServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureMediator(this IServiceCollection services)
        {
            services.AddSingleton<IInfrastructureMediator, InfrastructureMediator>();

            return services;
        }
    }
}
