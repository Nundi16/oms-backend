using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Authorization.Guards;

namespace OMS.Common.Abstractions.Communication.Authorization.Guards
{
    public abstract class AuthorizationGuard(IUserContext context) : IAuthorizationGuard
    {
        protected abstract string RequiredRole { get; }
        public virtual IResult Authorize() => context.Claims.IsInRole(RequiredRole)
            ? Result.Success()
            : Result.Failure(Constants.Errors.AUTHORIZATION_FAILED);
    }
}
