using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Common;
using OMS.Common.Interfaces;

namespace OMS.Infrastructure.Configuration
{
    internal abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IDomainEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasQueryFilter(Constants.Infrastructure.DELETED_FILTER, e => !e.Deleted.HasValue);
            builder.Property<byte[]>(Constants.Infrastructure.ROW_VERSION).IsRowVersion();
        }
    }
}
