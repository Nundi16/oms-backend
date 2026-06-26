using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OMS.Application.Interfaces.Persistation;
using OMS.Common.Communication;
using OMS.Common.Interfaces;
using OMS.Infrastructure.Audit;
using OMS.Infrastructure.Authorization;
using OMS.Infrastructure.Filters.Helpers;
using OMS.Infrastructure.Interceptors;
using OMS.Infrastructure.Options;
using OMS.Infrastructure.Persistence;
using static OMS.Common.Constants.Infrastructure;

namespace OMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddHealthChecks(configuration);
            services.AddOptions();
            services.AddInternalAuthorization();
            services.AddSingleton(TimeProvider.System);
            services.RegisterHandlersFromCurrentAssembly();

            services.AddMediator();
            services.AddPersistence();

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISaveChangesInterceptor, AuditSaveChangesInterceptor>();
            services.AddDbContext<IDbContext, ApplicationDbContext>((provider, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString(DEFAULT_CONNECTION));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.AddInterceptors(provider.GetServices<AuditSaveChangesInterceptor>());
            });

            return services;
        }

        private static IServiceCollection AddOptions(this IServiceCollection services)
        {
            services.Configure<EntityStateActionOptions>(options =>
            {
                options.RegisterAction(EntityState.Added, ShadowPropertySetters.Creation.Set);
                options.RegisterAction(EntityState.Modified, ShadowPropertySetters.Modification.Set);
                options.RegisterAction(EntityState.Deleted, ShadowPropertySetters.Deletion.Set);
                options.Complete();
            });

            var filterOptions = Microsoft.Extensions.Options.Options.Create(new FilterOptions
            {
                All = FilterOptionsHelper.GetFilterOptionsFromCurrentAssembly()
            });

            services.AddSingleton(filterOptions);

            return services;
        }

        private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks().AddNpgSql(configuration.GetConnectionString(DEFAULT_CONNECTION) 
                ?? throw new InvalidOperationException($"Unable to resolve value for {DEFAULT_CONNECTION} from the configuration."));

            return services;
        }

        private static IServiceCollection AddInternalAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();

            return services;
        }
    }
}
