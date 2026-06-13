using System.Security.Claims;

namespace OMS.Common.Interfaces
{
    public interface IUserContext 
    {
        public Guid Id { get; }
        ClaimsPrincipal Claims { get; }
    }
}
