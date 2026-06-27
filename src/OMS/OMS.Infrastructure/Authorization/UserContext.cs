using System.Security.Claims;
using OMS.Common.Interfaces;

namespace OMS.Infrastructure.Authorization
{
    internal sealed class UserContext : IUserContext
    {
        public Guid Id => Guid.NewGuid();
        public ClaimsPrincipal Claims => new();
    }
}
