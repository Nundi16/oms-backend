using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OMS.Infrastructure.Configuration;

namespace OMS.Infrastructure
{
    internal class ApplicationDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(EntityConfiguration<>).Assembly);
        }
    }
}
