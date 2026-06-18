using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Common.Abstractions.Entity;
using static OMS.Common.Constants.Infrastructure;

namespace OMS.Infrastructure.Abstractions.Configuration
{
    internal abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.HasQueryFilter(
                LOGICAL_DELETION_FILTER_KEY, 
                entity => !EF.Property<DateTime?>(entity, ShadowProperties.DELETED_AT).HasValue
                );
            builder.Property<byte[]>(ShadowProperties.ROW_VERSION).IsRowVersion();
            builder.Property<Guid>(ShadowProperties.CREATOR_ID).IsRequired();
            builder.Property<Guid?>(ShadowProperties.MODIFIER_ID).IsRequired(false);
            builder.Property<DateTime>(ShadowProperties.CREATED_AT).IsRequired();
            builder.Property<DateTime?>(ShadowProperties.MODIFIED_AT).IsRequired(false);
            builder.Property<DateTime?>(ShadowProperties.DELETED_AT).IsRequired(false);
        }
    }
}
