using OMS.Common.Filter.Enums;

namespace OMS.Application.Models
{
    public class FilterTypeDto
    {
        public string Module { get; set; }
        public FilterOperator FilterOperator { get; set; }
        public string FilterValueType { get; set; }
        public string Field { get; set; }
    }
}
