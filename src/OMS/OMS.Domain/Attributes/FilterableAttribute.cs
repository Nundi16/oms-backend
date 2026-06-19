using System.Diagnostics.CodeAnalysis;

namespace OMS.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class FilterableAttribute : Attribute
    {
        public readonly string? RoleName;
        private readonly bool _requiresAuthorization;

        [MemberNotNullWhen(true, nameof(RoleName))]
        public bool RequiresAuthorization => _requiresAuthorization;

        public FilterableAttribute()
        { }

        public FilterableAttribute(string roleName)
        {
            RoleName = roleName;
            _requiresAuthorization = true;
        }
    }
}
