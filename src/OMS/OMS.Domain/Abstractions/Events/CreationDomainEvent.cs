using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public abstract record CreationDomainEvent<TEntity> 
        : DomainEvent<TEntity>,
        ICreationDomainEvent<TEntity>
        where TEntity : Entity;
}
