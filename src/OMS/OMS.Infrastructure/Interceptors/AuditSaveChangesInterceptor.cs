using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Infrastructure.Options;

namespace OMS.Infrastructure.Interceptors
{
    internal sealed class AuditSaveChangesInterceptor(IUserContext user, TimeProvider timeProvider, IOptions<EntityStateActionOptions> options) 
        : SaveChangesInterceptor
    {
        private readonly EntityStateActionOptions _options = options.Value;

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eventData.Context);

            SetAuditProperties(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ArgumentNullException.ThrowIfNull(eventData.Context);

            SetAuditProperties(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        private void SetAuditProperties(DbContext context)
        {
            var entries = GetEntries(context, _options.RegisteredStates);
            InvokeActions(entries, _options, user.Id, timeProvider.GetUtcNow().UtcDateTime);
        }

        private static EntityEntry<Entity>[] GetEntries(DbContext context, IReadOnlyCollection<EntityState> states) =>
            [.. context.ChangeTracker.Entries<Entity>().Where(e => states.Contains(e.State))];

        private static void InvokeActions(EntityEntry<Entity>[] entries, EntityStateActionOptions options, Guid userId, DateTime dateTime)
        {
            foreach (var entry in entries)
            {
                options.GetAction(entry.State).Invoke(entry, userId, dateTime);
            }
        }
    }
}
