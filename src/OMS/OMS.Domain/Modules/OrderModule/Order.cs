using OMS.Common.Abstractions.Entity;
using OMS.Common.Attributes;
using OMS.Common.Filter.Enums;

namespace OMS.Domain.Modules.OrderModule
{
    public sealed class Order : Entity
    {
        [Filterable(nameof(Name), FilterOperator.Contains)]
        public string Name { get; set; } = string.Empty;

        // Dynamic dropdown value (runtime-managed catalog) - stored as code/key
        [Filterable(nameof(Status), FilterOperator.Equals)]
        public string? Status { get; set; }

        public DateTime? DeliveryDate { get; set; }
        [Filterable(nameof(DeliveryLocation), FilterOperator.Contains)]
        public string? DeliveryLocation { get; set; }

        public string? Notes { get; set; }

        // Dynamic dropdown value (runtime-managed catalog) - stored as code/key
        public string? Product { get; set; }

        public DateTime? ScannedAt { get; set; }

        public string? PatientNo { get; set; }
    }
}
