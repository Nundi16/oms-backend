using OMS.Common.Interfaces.Communication.Handlers;

namespace OMS.Common.Interfaces.Communication.Authorization.Guards
{
    public interface IMediatorAuthorizationGuard
    {
        IResult Authorize<TEventHandler, TEvent>(TEventHandler handler)
            where TEventHandler : IHandler
            where TEvent : class;

        IResult Authorize<TEventHandler>(TEventHandler handler)
            where TEventHandler : IHandler;
    }
}
