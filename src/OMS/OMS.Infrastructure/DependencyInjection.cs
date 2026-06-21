using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Communication.Requests;
using OMS.Application.Communication.Responses;
using OMS.Application.Interfaces.Persistence;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Communication;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Infrastructure.Audit;
using OMS.Infrastructure.Authorization;
using OMS.Infrastructure.Communication;
using OMS.Infrastructure.Communication.Handlers;
using OMS.Infrastructure.Interceptors;
using OMS.Infrastructure.Interfaces.Communication.Handlers;
using OMS.Infrastructure.Options;
using OMS.Infrastructure.Persistence;
using System.Reflection;
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

            // Register generic CRUD handlers first as fallback
            services.RegisterGenericHandlers();

            // Register infrastructure-level domain event handlers used by InfrastructureMediator
            services.RegisterInfrastructureDomainEventHandlers();

            // Connector handlers and unit of work participate in the mediator pipeline.
            services.RegisterConnectorPipeline();

            // Then register specific handlers from this assembly
            // (this picks up the OrderClinic* authorized event handlers automatically).
            services.RegisterHandlersFromCurrentAssembly();

            services.AddMediator();
            services.AddInfrastructureMediator();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISaveChangesInterceptor, AuditSaveChangesInterceptor>();
            services.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString(DEFAULT_CONNECTION));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.AddInterceptors(provider.GetServices<IInterceptor>());
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

            return services;
        }

        private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks().AddNpgSql(configuration.GetConnectionString(DEFAULT_CONNECTION) 
                ?? throw new InvalidOperationException($"Unable to resolve value for {DEFAULT_CONNECTION} from the configuration."));

            return services;
        }

        private static IServiceCollection RegisterGenericHandlers(this IServiceCollection services)
        {
            var domainAssemblyTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name?.StartsWith("OMS.Domain") == true)
                .SelectMany(a => a.GetTypes());

            var entityTypes = domainAssemblyTypes
                .Where(t => !t.IsAbstract && t.IsClass && typeof(OMS.Common.Abstractions.Entity.Entity).IsAssignableFrom(t))
                .ToList();

            foreach (var entityType in entityTypes)
            {
                var responseType = typeof(EntityResponse<>).MakeGenericType(entityType);
                // Register CreateEntityRequestHandler<TEntity>
                var createHandlerInterface = typeof(IRequestHandler<,>).MakeGenericType(
                    typeof(CreateEntityRequest<>).MakeGenericType(entityType), responseType);
                services.AddScoped(createHandlerInterface, typeof(CreateEntityRequestHandler<>).MakeGenericType(entityType));

                // Register UpdateEntityRequestHandler<TEntity>
                var updateRequestType = typeof(UpdateEntityRequest<>).MakeGenericType(entityType);
                var updateHandlerInterface = typeof(IRequestHandler<,>).MakeGenericType(updateRequestType, responseType);
                var updateHandlerImplementation = typeof(UpdateEntityRequestHandler<>).MakeGenericType(entityType);
                services.AddScoped(updateHandlerInterface, updateHandlerImplementation);

                // Register DeleteEntityRequestHandler<TEntity>
                var deleteRequestType = typeof(DeleteEntityRequest<>).MakeGenericType(entityType);
                var deleteHandlerInterface = typeof(IRequestHandler<,>).MakeGenericType(deleteRequestType, typeof(DeleteEntityResponse));
                var deleteHandlerImplementation = typeof(DeleteEntityRequestHandler<>).MakeGenericType(entityType);
                services.AddScoped(deleteHandlerInterface, deleteHandlerImplementation);

                // Register GetEntityRequestHandler<TEntity>
                var getRequestType = typeof(GetEntityRequest<>).MakeGenericType(entityType);
                var getHandlerInterface = typeof(IRequestHandler<,>).MakeGenericType(getRequestType, responseType);
                var getHandlerImplementation = typeof(GetEntityRequestHandler<>).MakeGenericType(entityType);
                services.AddScoped(getHandlerInterface, getHandlerImplementation);

                // Register GetEntitiesRequestHandler<TEntity>
                var getListRequestType = typeof(GetEntitiesRequest<>).MakeGenericType(entityType);
                var getListResponseType = typeof(EntityListResponse<>).MakeGenericType(entityType);
                var getListHandlerInterface = typeof(IRequestHandler<,>).MakeGenericType(getListRequestType, getListResponseType);
                var getListHandlerImplementation = typeof(GetEntitiesRequestHandler<>).MakeGenericType(entityType);
                services.AddScoped(getListHandlerInterface, getListHandlerImplementation);
            }

            return services;
        }

        private static IServiceCollection RegisterInfrastructureDomainEventHandlers(this IServiceCollection services)
        {
            services.AddScoped<ICreationDomainEventHandler, CreationEventHandler>();
            services.AddScoped<IDeletionDomainEventHandler, DeletionEventHandler>();
            services.AddScoped<IModificationEventHandler, ModificationEventHandler>();

            return services;
        }

        private static IServiceCollection AddInternalAuthorization(this IServiceCollection services)
        {
            // IUserContext is contributed by the Presentation layer (HTTP-bound).
            services.AddScoped<OMS.Application.Interfaces.Authorization.ICurrentClinicProvider, StaticCurrentClinicProvider>();

            return services;
        }

        private static IServiceCollection RegisterConnectorPipeline(this IServiceCollection services)
        {
            // Single persistence flush boundary used by Application CRUD handlers
            // to commit connector mutations after fan-out completes.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Connector handlers are discovered automatically by the assembly scan
            // because they implement IEventHandler<TContext>. Their authorization
            // guards (e.g. ModuleRuleGuard) are constructed inline at the handler
            // call site, so no extra DI registration is needed here.
            return services;
        }
    }
}
