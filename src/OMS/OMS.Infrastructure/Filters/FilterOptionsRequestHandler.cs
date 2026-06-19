using Microsoft.Extensions.Options;
using OMS.Common;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Handlers;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Infrastructure.Filters.Models;
using OMS.Infrastructure.Options;

namespace OMS.Infrastructure.Filters
{
    internal class FilterOptionsRequestHandler(IOptions<FilterOptions> options, IUserContext context) : IRequestHandler<FilterOptionsResponse>, IScopedHandler
    {
        public Task<IResult<FilterOptionsResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var filterOptions = options.Value.All
                .Where(option => 
                    !option.RequiresAuthorization || 
                    context.Claims.IsInRole(option.RequiresRole))
                .ToArray();

            return Task.FromResult(Result.Success(new FilterOptionsResponse { Options = filterOptions }));
        }
    }
}
