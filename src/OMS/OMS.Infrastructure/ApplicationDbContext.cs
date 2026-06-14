using Microsoft.EntityFrameworkCore;
using OMS.Infrastructure.Abstractions.Configuration;

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
