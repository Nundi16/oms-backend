using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OMS.Common.Interfaces;

namespace OMS.Infrastructure.Authorization
{
    internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        private const string SubjectClaimType = "sub";

        public ClaimsPrincipal Claims => httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

        public Guid Id => Guid.TryParse(
            Claims.FindFirst(SubjectClaimType)?.Value ?? Claims.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out var id)
            ? id
            : Guid.Empty;
    }
}
