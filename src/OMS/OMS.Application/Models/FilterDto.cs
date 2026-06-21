using OMS.Common.Filter.Enums;

namespace OMS.Application.Models
{
    public class FilterDto
    {
        public string Module { get; set; }
        public FilterOperator FilterOperator { get; set; }
        public string Field { get; set; }
        //TODO: This is not cool
        public string Value { get; set; }
    }
}
