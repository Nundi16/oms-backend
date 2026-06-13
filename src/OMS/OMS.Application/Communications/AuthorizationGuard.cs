using OMS.Common;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using System.Security.Claims;

namespace OMS.Application.Communications
{
    public abstract class AuthorizationGuard(ClaimsPrincipal claims) : IHandlerAuthorizationGuard
    {
        protected abstract string RequiredClaimType { get; }

        public IResult Authorize() =>
            claims.FindFirst(RequiredClaimType)?.Value is not null ? Result.Success() : Result.Failure(Constants.Errors.AUTHORIZATION_FAILED);
    }
}
