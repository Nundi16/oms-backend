using OMS.Common.Interfaces;

namespace OMS.Domain
{
    public abstract class Entity : IDomainEntity
    {
        public Guid Id { get; set; }
        public Guid Creator { get; set; }
        public DateTime Created { get; set; }
        public Guid LastModifier { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
