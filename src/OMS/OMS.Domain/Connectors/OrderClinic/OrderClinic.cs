using OMS.Domain.Modules;
namespace OMS.Domain.Connectors
{
    public class OrderClinic : Connector<Order, Clinic>
    {
        public string ClinicSpecificOrderName { get; set; }
    }
}
