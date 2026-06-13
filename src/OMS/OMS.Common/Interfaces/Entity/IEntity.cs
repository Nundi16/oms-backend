namespace OMS.Common.Interfaces.Entity
{
    public interface IEntity<TKey> where TKey : struct, IComparable, IComparable<TKey>, IEquatable<TKey>, IFormattable
    {
        TKey Id { get; }
    }
}
