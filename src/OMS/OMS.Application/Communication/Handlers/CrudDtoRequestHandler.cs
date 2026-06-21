using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OMS.Application.Common.Interfaces;
using OMS.Application.Communication.Requests;
using OMS.Application.Communication.Responses;
using OMS.Application.Connectors;
using OMS.Application.Connectors.Abstractions;
using OMS.Application.Connectors.Pipeline;
using OMS.Application.Interfaces.Persistence;
using OMS.Application.Models;
using OMS.Common;
using OMS.Common.Abstractions.Communication.Authorization.Guards;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Attributes;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers;
using OMS.Common.Interfaces.Communication.Handlers.Event;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Domain.Connectors;
using OMS.Domain.Connectors.OrderClinicConnector;
using System.Reflection;

namespace OMS.Application.Communication.Handlers
{
    internal sealed class BaseCreateRequestDtoHandler<TEntity, TDto>(
        IMediator mediator,
        IMapper mapper,
        IUnitOfWork unitOfWork)
        : IScopedHandler, IRequestHandler<BaseCreateRequestDto<TEntity, TDto>, BaseResponseDto<TEntity>>
        where TEntity : Entity
        where TDto : IDto<TEntity>
    {
        public async Task<IResult<BaseResponseDto<TEntity>>> HandleAsync(BaseCreateRequestDto<TEntity, TDto> @event, CancellationToken cancellationToken = default)
        {
            var entity = mapper.Map<TEntity>(@event.Payload);

            // Create the parent entity. The generic infrastructure handler issues its own
            // SaveChangesAsync because we need the generated Id before fanning out to
            // connector handlers. POC compromise — production should wrap parent + connectors
            // in a single IDbContextTransaction.
            var result = await mediator.RequestAsync<CreateEntityRequest<TEntity>, EntityResponse<TEntity>>(
                new CreateEntityRequest<TEntity>(entity),
                cancellationToken);

            if (!result.Succeeded || result.Value is null)
            {
                return Result.Failure<BaseResponseDto<TEntity>>(result.ErrorMessage ?? "Failed to create entity.");
            }

            var parentId = result.Value.Entity.Id;

            // Fan out connector persistence. Authorized handlers schedule Add/Update/Remove
            // on the shared DbContext but DO NOT save themselves; we flush once below.
            if (@event.Payload is IConnectorsCarrier carrier && carrier.Connectors is { Count: > 0 })
            {
                await mediator.FanOutSequentialAsync(
                    new ConnectorPersistContext<TEntity>(parentId, carrier.Connectors),
                    cancellationToken);

                await unitOfWork.SaveAsync(cancellationToken);
            }

            var dto = mapper.Map<TDto>(result.Value.Entity);

            // Re-load connectors so the response reflects the persisted state.
            if (dto is IConnectorsCarrier dtoCarrier)
            {
                dtoCarrier.Connectors = await LoadConnectorsAsync(parentId, cancellationToken);
            }

            return Result.Success(new BaseResponseDto<TEntity>(dto));
        }

        private async Task<IReadOnlyList<BaseConnectorDto>> LoadConnectorsAsync(Guid parentId, CancellationToken cancellationToken)
        {
            var ctx = new ConnectorLoadContext<TEntity>(new[] { parentId });
            await mediator.FanOutSequentialAsync(ctx, cancellationToken);
            return ctx.Accumulator.TryGetValue(parentId, out var bucket)
                ? bucket
                : new List<BaseConnectorDto>();
        }
    }

    internal sealed class BaseGetByIdRequestDtoHandler<TEntity, TDto>(
        IMediator mediator,
        IMapper mapper)
        : IScopedHandler, IRequestHandler<BaseGetByIdRequestDto<TEntity, TDto>, BaseResponseDto<TEntity>>
        where TEntity : Entity
        where TDto : IDto<TEntity>
    {
        public async Task<IResult<BaseResponseDto<TEntity>>> HandleAsync(BaseGetByIdRequestDto<TEntity, TDto> @event, CancellationToken cancellationToken = default)
        {
            var result = await mediator.RequestAsync<GetEntityRequest<TEntity>, EntityResponse<TEntity>>(
                new GetEntityRequest<TEntity>(@event.Id),
                cancellationToken);

            if (!result.Succeeded || result.Value is null)
            {
                return Result.Failure<BaseResponseDto<TEntity>>(result.ErrorMessage ?? "Entity not found.");
            }

            var dto = mapper.Map<TDto>(result.Value.Entity);

            if (dto is IConnectorsCarrier carrier)
            {
                var ctx = new ConnectorLoadContext<TEntity>(new[] { result.Value.Entity.Id });
                await mediator.FanOutSequentialAsync(ctx, cancellationToken);

                carrier.Connectors = ctx.Accumulator.TryGetValue(result.Value.Entity.Id, out var bucket)
                    ? bucket
                    : new List<BaseConnectorDto>();
            }

            return Result.Success(new BaseResponseDto<TEntity>(dto));
        }
    }

    internal sealed class BaseGetAllRequestDtoHandler<TEntity, TDto>(
        IMediator mediator,
        IMapper mapper)
        : IScopedHandler, IRequestHandler<BaseGetAllRequestDto<TEntity, TDto>, BaseListResponseDto<TDto>>
        where TEntity : Entity
        where TDto : IDto<TEntity>
    {
        public async Task<IResult<BaseListResponseDto<TDto>>> HandleAsync(BaseGetAllRequestDto<TEntity, TDto> @event, CancellationToken cancellationToken = default)
        {
            var result = await mediator.RequestAsync<GetEntitiesRequest<TEntity>, EntityListResponse<TEntity>>(
                new GetEntitiesRequest<TEntity>(),
                cancellationToken);

            if (!result.Succeeded || result.Value is null)
            {
                return Result.Failure<BaseListResponseDto<TDto>>(result.ErrorMessage ?? "Failed to query entities.");
            }

            var dtos = mapper.Map<IReadOnlyList<TDto>>(result.Value.Entities);

            if (dtos.FirstOrDefault() is IConnectorsCarrier)
            {
                var entityIds = result.Value.Entities.Select(e => e.Id).ToList();
                var ctx = new ConnectorLoadContext<TEntity>(entityIds);
                await mediator.FanOutSequentialAsync(ctx, cancellationToken);

                for (int i = 0; i < result.Value.Entities.Count; i++)
                {
                    var entityId = result.Value.Entities[i].Id;
                    if (dtos[i] is IConnectorsCarrier dtoCarrier)
                    {
                        dtoCarrier.Connectors = ctx.Accumulator.TryGetValue(entityId, out var bucket)
                            ? bucket
                            : new List<BaseConnectorDto>();
                    }
                }
            }

            return Result.Success(new BaseListResponseDto<TDto>(dtos));
        }
    }

    internal sealed class BaseUpdateRequestDtoHandler<TEntity, TDto>(
        IMediator mediator,
        IMapper mapper,
        IUnitOfWork unitOfWork)
        : IScopedHandler, IRequestHandler<BaseUpdateRequestDto<TEntity, TDto>, BaseResponseDto<TEntity>>
        where TEntity : Entity
        where TDto : IDto<TEntity>
    {
        public async Task<IResult<BaseResponseDto<TEntity>>> HandleAsync(BaseUpdateRequestDto<TEntity, TDto> @event, CancellationToken cancellationToken = default)
        {
            if (@event.Payload.Id == Guid.Empty)
                return Result.Failure<BaseResponseDto<TEntity>>("Invalid entity ID.");


            var entity = mapper.Map<TEntity>(@event.Payload);
            var result = await mediator.RequestAsync<UpdateEntityRequest<TEntity>, EntityResponse<TEntity>>(
                new UpdateEntityRequest<TEntity>(@event.Payload.Id.Value, entity),
                cancellationToken);

            if (!result.Succeeded || result.Value is null)
            {
                return Result.Failure<BaseResponseDto<TEntity>>(result.ErrorMessage ?? "Failed to update entity.");
            }

            // Fan out connector persistence — replace semantics, single SaveAsync at the end.
            if (@event.Payload is IConnectorsCarrier carrier && carrier.Connectors is not null)
            {
                await mediator.FanOutSequentialAsync(
                    new ConnectorPersistContext<TEntity>(@event.Payload.Id.Value, carrier.Connectors),
                    cancellationToken);

                await unitOfWork.SaveAsync(cancellationToken);
            }

            var dto = mapper.Map<TDto>(result.Value.Entity);

            if (dto is IConnectorsCarrier dtoCarrier)
            {
                var ctx = new ConnectorLoadContext<TEntity>(new[] { @event.Payload.Id.Value });
                await mediator.FanOutSequentialAsync(ctx, cancellationToken);

                dtoCarrier.Connectors = ctx.Accumulator.TryGetValue(@event.Payload.Id.Value, out var bucket)
                    ? bucket
                    : new List<BaseConnectorDto>();
            }

            return Result.Success(new BaseResponseDto<TEntity>(dto));
        }
    }

    internal sealed class BaseDeleteRequestDtoHandler<TEntity, TDto>(IMediator mediator)
        : IScopedHandler, IRequestHandler<BaseDeleteRequestDto<TEntity, TDto>, BaseDeleteResponseDto>
        where TEntity : Entity
        where TDto : IDto<TEntity>
    {
        public async Task<IResult<BaseDeleteResponseDto>> HandleAsync(BaseDeleteRequestDto<TEntity, TDto> @event, CancellationToken cancellationToken = default)
        {
            var result = await mediator.RequestAsync<DeleteEntityRequest<TEntity>, DeleteEntityResponse>(
                new DeleteEntityRequest<TEntity>(@event.Id),
                cancellationToken);

            if (!result.Succeeded || result.Value is null)
            {
                return Result.Failure<BaseDeleteResponseDto>(result.ErrorMessage ?? "Failed to delete entity.");
            }

            return Result.Success(new BaseDeleteResponseDto(result.Value.Id, result.Value.Success));
        }
    }

    internal interface IFilterHandler
    {
        Task<IResult<BaseFilterResponseDto>> HandleAsync(object @event,
            CancellationToken cancellationToken = default);
    }

    internal sealed class GetAllFilterHandler<TEntity>(IMediator mediator, IServiceProvider serviceProvider) :
        IRequestHandler<BaseFilterRequestDto<TEntity>, BaseFilterResponseDto>, IFilterHandler
        where TEntity : Entity
    {
        public async Task<IResult<BaseFilterResponseDto>> HandleAsync(object @event, CancellationToken cancellationToken = default)
        {
            return await HandleAsync((BaseFilterRequestDto<TEntity>)@event, cancellationToken);
        }

        //4Robi - Tudom hogy reflection de nem érdekel :P
        public async Task<IResult<BaseFilterResponseDto>> HandleAsync(BaseFilterRequestDto<TEntity> @event, CancellationToken cancellationToken = default)
        {
            List<FilterTypeDto> filters = [];

            //Get all filter from the entity
            var filterAttributes = typeof(TEntity).GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(FilterableAttribute), true).Any())
                .Select(x => x.GetCustomAttribute<FilterableAttribute>()).ToList();

            filters.AddRange(filterAttributes.Select(a => new FilterTypeDto
            {
                Field = a.Field,
                FilterOperator = a.Operator,
                Module = typeof(TEntity).Name,
                FilterValueType = typeof(string).Name
            }));

            //Get all connector for the entity - only works for direct inheritance
            var connectorTypes = typeof(TEntity).Assembly.GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t =>
                    t.BaseType?.IsGenericType == true &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(Connector<,>) &&
                    t.BaseType.GetGenericArguments()[0] == typeof(TEntity))
                .ToList();


            //Get all filter from connectors via connectorhandlers
            foreach (var connType in connectorTypes)
            {
                var connectorRequestType = typeof(BaseFilterRequestDto<>).MakeGenericType(connType);
                var connectorFilterType = typeof(IRequestHandler<,>).MakeGenericType(connectorRequestType, typeof(BaseFilterResponseDto));
                var connectorHandler = (IFilterHandler)serviceProvider.GetRequiredService(connectorFilterType);

                //This is not nice...
                var connectorFilters = await connectorHandler.HandleAsync(Activator.CreateInstance(connectorRequestType));
                filters.AddRange(connectorFilters.Value.Filters);
            }

            return Result.Success(new BaseFilterResponseDto(filters));
        }
    }
}
