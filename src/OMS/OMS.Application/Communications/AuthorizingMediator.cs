using Microsoft.Extensions.DependencyInjection;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;

namespace OMS.Application.Communications
{
    public class AuthorizingMediator(IServiceProvider serviceProvider, IMediatorAuthorizationGuard authorizationGuard) : IAuthorizingMediator
    {
        public Task EmitAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
        {
            var handler = serviceProvider.GetRequiredService<IEventHandler<TEvent>>();

            return authorizationGuard.Authorize<IEventHandler<TEvent>, TEvent>(handler).Succeeded
                ? handler.HandleAsync(@event, cancellationToken)
                : Task.CompletedTask;
        }

        public Task FanOutAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
        {
            var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>()
                .Where(handler => authorizationGuard.Authorize<IEventHandler<TEvent>, TEvent>(handler).Succeeded);

            var tasks = handlers.Select(handler => handler.HandleAsync(@event, cancellationToken)).ToArray();

            return Task.WhenAll(tasks);
        }

        public Task<IResult<TResponse>> RequestAsync<TEvent, TResponse>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class
            where TResponse : class
        {
            var handler = serviceProvider.GetRequiredService<IRequestHandler<TEvent, TResponse>>();

            return authorizationGuard.Authorize<IRequestHandler<TEvent, TResponse>, TEvent>(handler).Succeeded
                ? handler.HandleAsync(@event, cancellationToken)
                : Task.FromResult<IResult<TResponse?>>(default);
        }
    }
}
