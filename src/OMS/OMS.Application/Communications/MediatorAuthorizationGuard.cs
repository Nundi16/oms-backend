using OMS.Common;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;

namespace OMS.Application.Communications
{
    public class MediatorAuthorizationGuard : IMediatorAuthorizationGuard
    {
        public IResult Authorize<TEventHandler, TEvent>(TEventHandler handler)
            where TEvent : class
            where TEventHandler : IHandler =>
            handler is IAuthorizedEventHandler<TEvent> authorizedHandler
                ? authorizedHandler.Authorize()
                : Result.Success();
    }
}
