namespace OMS.Application.Connectors
{
    public class BaseConnectorDto
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid DependantId { get; set; }
    }
}
