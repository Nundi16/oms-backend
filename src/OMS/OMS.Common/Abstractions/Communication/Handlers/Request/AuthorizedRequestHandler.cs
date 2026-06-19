using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Common.Interfaces.Communication.Handlers.Request;

namespace OMS.Common.Abstractions.Communication.Handlers.Request
{
    public abstract class AuthorizedRequestHandler<TRequest, TResponse, TAuthorizationGuard>(TAuthorizationGuard guard)
        : AuthorizedHandler<TAuthorizationGuard>(guard),
        IRequestHandler<TRequest, TResponse>
        where TRequest : class
        where TAuthorizationGuard : IAuthorizationGuard
    {
        public abstract Task<IResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }

    public abstract class AuthorizedRequestHandler<TResponse, TAuthorizationGuard>(TAuthorizationGuard guard)
        : AuthorizedHandler<TAuthorizationGuard>(guard),
        IRequestHandler<TResponse>
        where TAuthorizationGuard : IAuthorizationGuard
    {
        public abstract Task<IResult<TResponse>> HandleAsync(CancellationToken cancellationToken = default);
    }
}
