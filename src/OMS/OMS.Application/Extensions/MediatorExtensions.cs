using OMS.Application.Models;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Entity;
using OMS.Domain.Abstractions.Events;

namespace OMS.Application.Extensions
{
    internal static class MediatorExtensions
    {
        internal static Task<IResult<ServiceResponse<TEntity>>> RequestAsync<TEntity>(
            this IMediator mediator, 
            CreationDomainEvent<TEntity> request, 
            CancellationToken cancellationToken = default)
            where TEntity : IEntity<Guid>
            => mediator.RequestAsync<CreationDomainEvent<TEntity>, ServiceResponse<TEntity>>(request, cancellationToken);

        internal static Task<IResult<ServiceResponse<TEntity>>> RequestAsync<TEntity>(
            this IMediator mediator,
            ModificationDomainEvent<TEntity> request,
            CancellationToken cancellationToken = default)
            where TEntity : IEntity<Guid>
            => mediator.RequestAsync<ModificationDomainEvent<TEntity>, ServiceResponse<TEntity>>(request, cancellationToken);

        internal static Task<IResult<ServiceResponse<TEntity>>> RequestAsync<TEntity>(
            this IMediator mediator,
            DeletionDomainEvent<TEntity> request,
            CancellationToken cancellationToken = default)
            where TEntity : IEntity<Guid>
            => mediator.RequestAsync<DeletionDomainEvent<TEntity>, ServiceResponse<TEntity>>(request, cancellationToken);
    }
}
