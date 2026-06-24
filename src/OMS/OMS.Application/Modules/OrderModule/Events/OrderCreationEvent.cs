using OMS.Domain.Abstractions.Events;
using OMS.Domain.Modules.OrderModule;

namespace OMS.Application.Modules.OrderModule.Events
{
    internal record OrderCreationEvent(Order Entity) : CreationDomainEvent<Order>(Entity, []);
}
