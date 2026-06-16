using Microsoft.Extensions.DependencyInjection;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Common.Interfaces.Communication.Handlers.Event;
using OMS.Common.Interfaces.Communication.Handlers.Request;

namespace OMS.Common.Communication
{
    public class AuthorizingMediator(IServiceProvider serviceProvider, IMediatorAuthorizationGuard authorizationGuard) : IMediator
    {
        protected readonly IServiceProvider ServiceProvider = serviceProvider;
        protected readonly IMediatorAuthorizationGuard AuthorizationGuard = authorizationGuard;

        public Task EmitAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
        {
            var handler = ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();

            return AuthorizationGuard.Authorize<IEventHandler<TEvent>, TEvent>(handler).Succeeded
                ? handler.HandleAsync(@event, cancellationToken)
                : Task.CompletedTask;
        }

        public Task FanOutAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
        {
            var handlers = ServiceProvider.GetServices<IEventHandler<TEvent>>()
                .Where(handler => AuthorizationGuard.Authorize<IEventHandler<TEvent>, TEvent>(handler).Succeeded);

            var tasks = handlers.Select(handler => handler.HandleAsync(@event, cancellationToken)).ToArray();

            return Task.WhenAll(tasks);
        }

        public Task<IResult<TResponse>> RequestAsync<TEvent, TResponse>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class
            where TResponse : class
        {
            var handler = ServiceProvider.GetRequiredService<IRequestHandler<TEvent, TResponse>>();

            return AuthorizationGuard.Authorize<IRequestHandler<TEvent, TResponse>, TEvent>(handler).Succeeded
                ? handler.HandleAsync(@event, cancellationToken)
                : Task.FromResult<IResult<TResponse>>(Result.Failure<TResponse>(""));
        }
    }
}
