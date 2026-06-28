using OMS.Common.Abstractions.Entity;
using OMS.Domain.Modules.ClinicModule;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Domain.Modules.OrderClinicConnector
{
    public sealed class OrderClinic : ConnectorEntity<Order, Clinic>
    {
        public override string TypeDescriptor => nameof(OrderClinic);
        public string ClinicSpecificOrderName { get; set; }
    }
}
