using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Common.Interfaces.Communication.Handlers.Request;

namespace OMS.Common.Abstractions.Communication.Handlers.Request
{
    public abstract class AuthorizedRequestHandler<TEvent, TResponse, TAuthorizationGuard>(TAuthorizationGuard guard)
        : AuthorizedHandler<TAuthorizationGuard>(guard),
        IRequestHandler<TEvent, TResponse>
        where TEvent : class
        where TAuthorizationGuard : IAuthorizationGuard
    {
        public abstract Task<IResult<TResponse>> HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}
