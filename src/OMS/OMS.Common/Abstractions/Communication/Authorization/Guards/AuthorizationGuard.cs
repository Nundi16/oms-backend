using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication.Authorization.Guards;

namespace OMS.Common.Abstractions.Communication.Authorization.Guards
{
    public abstract class AuthorizationGuard(IUserContext context) : IAuthorizationGuard
    {
        protected abstract string RequiredClaimType { get; }
        public virtual IResult Authorize() => context.Claims.FindFirst(RequiredClaimType) is not null 
            ? Result.Success() 
            : Result.Failure("");
    }
}
