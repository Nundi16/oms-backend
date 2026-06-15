using Microsoft.EntityFrameworkCore.ChangeTracking;
using static OMS.Common.Constants.Infrastructure;

namespace OMS.Infrastructure.Audit
{
    internal class ModificationShadowPropertySetter
    {
        internal void Set(EntityEntry entry, Guid modifier, DateTime modifiedAt)
        {
            entry.Property(ShadowProperties.MODIFIER_ID).CurrentValue = modifier;
            entry.Property(ShadowProperties.MODIFIED_AT).CurrentValue = modifiedAt;
        }
    }
}
