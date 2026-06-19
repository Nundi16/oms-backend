using Microsoft.EntityFrameworkCore;
using OMS.Infrastructure.Abstractions.Configuration;

namespace OMS.Infrastructure
{
    /// <summary>
    /// Main database context for the OMS application.
    /// Made public to allow Application-layer connector readers/writers/filters direct access.
    /// TODO: Consider introducing per-module query accessors for better encapsulation in the future.
    /// </summary>
    public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityConfiguration<>).Assembly);
        }
    }
}
