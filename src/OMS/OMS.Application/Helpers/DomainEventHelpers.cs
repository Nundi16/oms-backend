using OMS.Application.Models;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Application.Helpers
{
    internal class DomainEventHelpers
    {
        internal static Task<IResult<ServiceResponse<IEntity<Guid>>>> InvokeHandler<TDomainEvent>(TDomainEvent entity, IMediator mediator) 
            where TDomainEvent : class, IDomainEvent<IEntity<Guid>> =>
            mediator.RequestAsync<TDomainEvent, ServiceResponse<IEntity<Guid>>>(entity);
    }
}
