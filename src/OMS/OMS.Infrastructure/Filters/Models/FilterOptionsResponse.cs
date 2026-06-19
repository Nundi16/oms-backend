using OMS.Infrastructure.Options;

namespace OMS.Infrastructure.Filters.Models
{
    public sealed record FilterOptionsResponse
    {
        public required FilterOption[] Options { get; init; }
    }
}
