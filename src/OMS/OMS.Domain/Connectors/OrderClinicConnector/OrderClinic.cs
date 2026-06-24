using OMS.Domain.Modules.ClinicModule;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Domain.Connectors.OrderClinicConnector
{
    public sealed class OrderClinic : Connector<Order, Clinic>
    {
        public override string TypeDescriptor => nameof(OrderClinic);
        public string ClinicSpecificOrderName { get; set; }
    }
}
