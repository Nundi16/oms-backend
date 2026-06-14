using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static OMS.Common.Constants.Infrastructure;

namespace OMS.Infrastructure.Audit
{
    internal static class ShadowPropertySetter
    {
        internal static void SetCreationProperties(EntityEntry entry, Guid creator, DateTime createdAt)
        {
            entry.Property(ShadowProperties.CREATOR_ID).CurrentValue = creator;
            entry.Property(ShadowProperties.CREATED_AT).CurrentValue = createdAt;
        }

        internal static void SetModificationProperties(EntityEntry entry, Guid modifier, DateTime modifiedAt)
        {
            entry.Property(ShadowProperties.MODIFIER_ID).CurrentValue = modifier;
            entry.Property(ShadowProperties.MODIFIED_AT).CurrentValue = modifiedAt;
        }

        internal static void SetDeletionProperties(EntityEntry entry, Guid modifier, DateTime deletedAt)
        {
            entry.Property(ShadowProperties.MODIFIER_ID).CurrentValue = modifier;
            entry.Property(ShadowProperties.DELETED_AT).CurrentValue = deletedAt;
            entry.State = EntityState.Modified;
        }
    }
}
