using OMS.Common.Abstractions.Entity;
using OMS.Domain.Interfaces.Events;

namespace OMS.Domain.Abstractions.Events
{
    internal abstract record ModificationDomainEvent<TEntity> 
        : DomainEvent<TEntity>,
        IModificationDomainEvent<TEntity>
        where TEntity : Entity;
}
