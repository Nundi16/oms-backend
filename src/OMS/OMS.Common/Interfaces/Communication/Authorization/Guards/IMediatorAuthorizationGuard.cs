using OMS.Common.Interfaces.Communication.Handlers;

namespace OMS.Common.Interfaces.Communication.Authorization.Guards
{
    public interface IMediatorAuthorizationGuard
    {
        IResult Authorize<TEventHandler>(TEventHandler handler)
            where TEventHandler : IHandler;
    }
}
