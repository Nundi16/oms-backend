using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddHealthChecks().AddNpgSql(_connectionString, name: "PostgreSQL");
        }
    }
}
