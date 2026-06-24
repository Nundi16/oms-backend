namespace OMS.Application.Interfaces.Persistation
{
    internal interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
