using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Domain.Connectors.OrderClinicConnector;
using OMS.Infrastructure.Abstractions.Configuration;

namespace OMS.Infrastructure.Modules.OrderClinicConnector.Configurations
{
	/// <summary>
	/// EF Core configuration for OrderClinic connector entity.
	/// Maps FK properties, ignores navigation properties, and defines indexes for query performance.
	/// </summary>
	internal sealed class OrderClinicConfiguration : EntityConfiguration<OrderClinic>
	{
		public override void Configure(EntityTypeBuilder<OrderClinic> builder)
		{
			base.Configure(builder);

			builder.ToTable("OrderClinics");

			// Foreign key properties (required)
			builder.Property(oc => oc.ParentId)
				.IsRequired();

			builder.Property(oc => oc.DependantId)
				.IsRequired();

			// Connector-specific property
			builder.Property(oc => oc.ClinicSpecificOrderName)
				.HasMaxLength(200)
				.IsRequired(false);

			// Navigation properties are explicitly ignored (FK-only updates)
			builder.Ignore(oc => oc.Parent);
			builder.Ignore(oc => oc.Dependant);

			// Indexes for query performance
			builder.HasIndex(oc => oc.ParentId)
				.HasDatabaseName("IX_OrderClinics_ParentId");

			builder.HasIndex(oc => oc.DependantId)
				.HasDatabaseName("IX_OrderClinics_DependantId");

			// Composite index for current clinic filter (ParentId + DependantId)
			builder.HasIndex(oc => new { oc.ParentId, oc.DependantId })
				.HasDatabaseName("IX_OrderClinics_ParentId_DependantId");
		}
	}
}
