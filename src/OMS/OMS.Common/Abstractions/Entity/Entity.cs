using OMS.Common.Interfaces.Entity;

namespace OMS.Common.Abstractions.Entity
{
    public abstract class Entity : IEntity<Guid>
    {
        public Guid Id { get; }
    }
}
