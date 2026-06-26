using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Domain.Connectors.OrderClinicConnector;
using OMS.Infrastructure.Abstractions.Configuration;

namespace OMS.Infrastructure.Connectors.OrderClinicConnector.Configurations
{
    internal sealed class OrderClinicConfiguration : EntityConfiguration<OrderClinic>
    {
        public override void Configure(EntityTypeBuilder<OrderClinic> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Source)
                .WithMany()
                .HasForeignKey(x => x.SourceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Dependant)
                .WithMany()
                .HasForeignKey(x => x.DependantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.SourceId, x.DependantId })
                .IsUnique();
        }
    }
}