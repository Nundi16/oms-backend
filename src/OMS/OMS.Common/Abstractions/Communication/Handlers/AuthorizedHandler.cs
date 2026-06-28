using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Authorization.Guards;
using OMS.Common.Interfaces.Communication.Handlers;

namespace OMS.Common.Abstractions.Communication.Handlers
{
    public abstract class AuthorizedHandler<TAuthorizationGuard>(TAuthorizationGuard guard) : IAuthorizedHandler
        where TAuthorizationGuard : IAuthorizationGuard
    {
        public virtual IResult Authorize() => guard.Authorize();
    }
}
