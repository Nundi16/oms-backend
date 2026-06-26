using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static OMS.Common.Constants.Infrastructure;

namespace OMS.Infrastructure.Audit
{
    internal class CreationShadowPropertySetter
    {
        internal void Set(EntityEntry entry, Guid creator, DateTime createdAt)
        {
            entry.Property(ShadowProperties.CREATOR_ID).CurrentValue = creator;
            entry.Property(ShadowProperties.CREATED_AT).CurrentValue = createdAt;
            entry.State = EntityState.Modified;
        }
    }
}
