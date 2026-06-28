namespace OMS.Common.Interfaces.Entity
{
    public interface IConnector<TKey> : IEntity<TKey>
        where TKey : struct, IComparable, IComparable<TKey>, IEquatable<TKey>, IFormattable
    {
        string TypeDescriptor { get; }
        TKey? SourceId { get; }
        TKey DependantId { get; }
        IConnector<TKey>[]? Connectors { get; set; }

        void AssignSourceId(Guid sourceId);
    }

    public interface IConnectorEntity : IConnector<Guid>;
}
