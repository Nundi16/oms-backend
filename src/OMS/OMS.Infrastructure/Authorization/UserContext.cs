using System.Security.Claims;
using OMS.Common.Interfaces;

namespace OMS.Infrastructure.Authorization
{
    internal sealed class UserContext : IUserContext
    {
        public Guid Id => throw new NotImplementedException();
        public ClaimsPrincipal Claims => throw new NotImplementedException();
    }
}
