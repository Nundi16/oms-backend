using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Common.Interfaces.Communication.Handlers.Event;

namespace OMS.Common.Abstractions.Communication.Handlers.Event
{
    public abstract class AuthorizedEventHandler<TEvent, TAuthorizationGuard>(TAuthorizationGuard guard)
        : AuthorizedHandler<TAuthorizationGuard>(guard),
        IEventHandler<TEvent>
        where TEvent : class
        where TAuthorizationGuard : IAuthorizationGuard
    {
        public abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}
