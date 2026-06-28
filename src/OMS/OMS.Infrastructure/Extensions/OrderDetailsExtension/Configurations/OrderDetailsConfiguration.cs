using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Domain.Extensions.OrderDetailsExtension;
using OMS.Infrastructure.Abstractions.Configuration;

namespace OMS.Infrastructure.Extensions.OrderDetailsExtension.Configurations
{
    internal sealed class OrderDetailsConfiguration : EntityConfiguration<OrderDetails>
    {
        public override void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Source)
                .WithMany()
                .HasForeignKey(x => x.SourceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
