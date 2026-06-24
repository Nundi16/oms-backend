using Microsoft.EntityFrameworkCore;
using OMS.Application.Interfaces.Persistation;
using OMS.Infrastructure.Abstractions.Configuration;

namespace OMS.Infrastructure
{
    internal sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityConfiguration<>).Assembly);
        }
    }
}
