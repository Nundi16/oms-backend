using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Common.Interfaces.Communication.Handlers;

namespace OMS.Common.Communication.Authorization.Guards
{
    internal class MediatorAuthorizationGuard : IMediatorAuthorizationGuard
    {
        public IResult Authorize<TEventHandler, TEvent>(TEventHandler handler)
            where TEventHandler : IHandler
            where TEvent : class
        {
            ArgumentNullException.ThrowIfNull(handler);

            return handler is IAuthorizedHandler authorizedHandler ? authorizedHandler.Authorize() : Result.Success();
        }
    }
}
