using OMS.Application.Interfaces.Persistence;

namespace OMS.Infrastructure.Persistence
{
	/// <summary>
	/// EF Core-backed unit of work. Delegates to <see cref="ApplicationDbContext.SaveChangesAsync"/>
	/// so that all pending entity changes (parent + connector tables) flush in a single
	/// transaction.
	/// </summary>
	internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
	{
		public Task<int> SaveAsync(CancellationToken cancellationToken = default)
			=> dbContext.SaveChangesAsync(cancellationToken);
	}
}
