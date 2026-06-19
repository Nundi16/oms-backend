using OMS.Common.Filter.Enums;

namespace OMS.Application.Attributes
{
    public class FilterAttribute
    {
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
        public class FilterableAttribute(string module, string roleName = null, string field = null,
            FilterOperator @operator = FilterOperator.Contains) : Attribute
        {
            public string? Role { get; set; }
            public string Module { get; set; }
            public string? Field { get; set; }
            public FilterOperator Operator { get; set; }
        }
    }
}
