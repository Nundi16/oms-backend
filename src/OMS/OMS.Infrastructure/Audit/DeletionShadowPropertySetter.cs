using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static OMS.Common.Constants.Infrastructure;

namespace OMS.Infrastructure.Audit
{
    internal class DeletionShadowPropertySetter
    {
        internal void Set(EntityEntry entry, Guid modifier, DateTime deletedAt)
        {
            entry.Property(ShadowProperties.MODIFIER_ID).CurrentValue = modifier;
            entry.Property(ShadowProperties.DELETED_AT).CurrentValue = deletedAt;
            entry.State = EntityState.Modified;
        }
    }
}
