using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Common.Interfaces.Communication.Handlers;

namespace OMS.Common.Communication.Authorization.Guards
{
    public class MediatorAuthorizationGuard : IMediatorAuthorizationGuard
    {
        public IResult Authorize<TEventHandler>(TEventHandler handler) where TEventHandler : IHandler =>
             handler is IAuthorizedHandler authorizedHandler ? authorizedHandler.Authorize() : Result.Success();
    }
}
