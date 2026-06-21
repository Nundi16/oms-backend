using OMS.Common.Filter.Enums;

namespace OMS.Application.Models
{
    public record Filter(string Name, FilterOperator FilterOperator, string Value);
}
