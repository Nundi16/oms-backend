using System.Security.Claims;

namespace OMS.Common.Interfaces
{
    public interface IUserContext 
    {
        Guid Id { get; }
        ClaimsPrincipal Claims { get; }
    }
}
