using OMS.Common.Attributes;
using OMS.Common.Filter.Enums;
using OMS.Domain.Modules.ClinicModule;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Domain.Connectors.OrderClinicConnector
{
    public sealed class OrderClinic : Connector<Order, Clinic>
    {
        [Filterable(nameof(ClinicSpecificOrderName), FilterOperator.Contains)]
        public string ClinicSpecificOrderName { get; set; }
    }
}
