using OMS.Common.Filter.Enums;

namespace OMS.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class FilterableAttribute(string field,
        FilterOperator @operator = FilterOperator.Contains, 
        string? roleName = null) : Attribute
    {
        public string? Role { get; set; } = roleName;
        public string Field { get; set; } = field;
        public FilterOperator Operator { get; set; } = @operator;
    }
}
