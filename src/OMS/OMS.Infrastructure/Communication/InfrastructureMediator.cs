using OMS.Application.Interfaces.Communication;
using OMS.Common.Communication;
using OMS.Common.Interfaces.Communication.Authorization.Guards;

namespace OMS.Infrastructure.Communication
{
    internal sealed class InfrastructureMediator(IServiceProvider serviceProvider, IMediatorAuthorizationGuard authorizationGuard) 
        : AuthorizingMediator(serviceProvider, authorizationGuard),
        IInfrastructureMediator
    {
    }
}
