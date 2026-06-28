using OMS.Application.Modules.OrderModule;
using OMS.Application.Modules.OrderModule.Authorization;
using OMS.Common.Abstractions.Communication.Authorization.Guards;
using OMS.Common.Interfaces;
using OMS.Domain;

namespace OMS.Infrastructure.Modules.OrderModule.Authorization
{
    public class OrderAuthorizationGuard(IUserContext context) : AuthorizationGuard(context), IOrderAuthorizationGuard
    {
        protected override string RequiredRole => Roles.Order;
    }
}
