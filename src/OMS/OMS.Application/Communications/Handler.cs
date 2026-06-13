using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;

namespace OMS.Application.Communications
{
    public abstract class AuthorizedHandler<TAuthorizationGuard>(TAuthorizationGuard guard) where TAuthorizationGuard : IHandlerAuthorizationGuard
    {
        public virtual IResult Authorize() => guard.Authorize();
    }

    public abstract class AuthorizedEventHandler<TAuthorizationGuard, TEvent>(TAuthorizationGuard guard) : AuthorizedHandler<TAuthorizationGuard>(guard), IAuthorizedEventHandler<TEvent> 
        where TEvent : class 
        where TAuthorizationGuard : IHandlerAuthorizationGuard
    {
        public abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }

    public abstract class AuthorizedRequestHandler<TAuthorizationGuard, TEvent, TResponse>(TAuthorizationGuard guard)
        : AuthorizedHandler<TAuthorizationGuard>(guard), IAuthorizedRequestHandler<TEvent, TResponse>
        where TEvent : class
        where TAuthorizationGuard : IHandlerAuthorizationGuard
    {
        public abstract Task<IResult<TResponse?>> HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}
