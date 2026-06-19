namespace OMS.Application.Connectors
{
    public class ConnectorBase
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid DependantId { get; set; }
    }
}
