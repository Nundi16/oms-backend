using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    public abstract record DeletionDomainEvent<TEntity> 
        : DomainEvent<TEntity>,
        IDeletionDomainEvent<TEntity>
        where TEntity : Entity;
}
