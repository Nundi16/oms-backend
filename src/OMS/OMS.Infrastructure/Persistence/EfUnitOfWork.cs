using OMS.Application.Interfaces.Persistation;

namespace OMS.Infrastructure.Persistence
{
    internal sealed class EfUnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            context.SaveChangesAsync(cancellationToken);
    }
}
