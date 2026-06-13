namespace OMS.Common.Interfaces
{
    public interface IDomainEntity 
    {
        Guid Id { get; set; }
        Guid Creator { get; set; }
        DateTime Created { get; set; }
        Guid LastModifier { get; set; }
        DateTime LastModified { get; set; }
        DateTime? Deleted { get; set; }
    }
}
