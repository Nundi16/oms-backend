using Microsoft.Extensions.Primitives;

namespace OMS.Presentation.Models.Filtering
{
    public class FilterContext(IHttpContextAccessor context)
    {
        private readonly StringValues Filters = context.HttpContext?.Request.Query["filters"] ?? [];

        public object GetFilter(string key)
        {
            return Filters.Where(filter => filter is not null && filter.StartsWith(key)).ToArray();
        }
    }
}
