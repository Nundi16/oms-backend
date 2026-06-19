using AutoMapper;
using OMS.Application.Communication.Requests;
using OMS.Application.Communication.Responses;
using OMS.Application.Connectors;
using OMS.Application.Connectors.Abstractions;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers;
using OMS.Common.Interfaces.Communication.Handlers.Request;

namespace OMS.Application.Communication.Handlers
{
	internal sealed class BaseCreateRequestDtoHandler<TEntity, TDto>(
		IMediator mediator,
		IMapper mapper,
		IEnumerable<IConnectorWriter<TEntity>> connectorWriters)
		: IScopedHandler, IRequestHandler<BaseCreateRequestDto<TDto>, BaseResponseDto<TDto>>
		where TEntity : Entity
		where TDto : class
	{
		public async Task<IResult<BaseResponseDto<TDto>>> HandleAsync(BaseCreateRequestDto<TDto> @event, CancellationToken cancellationToken = default)
		{
			var entity = mapper.Map<TEntity>(@event.Payload);
			var result = await mediator.RequestAsync<CreateEntityRequest<TEntity>, EntityResponse<TEntity>>(
				new CreateEntityRequest<TEntity>(entity),
				cancellationToken);

			if (!result.Succeeded || result.Value is null)
			{
				return Result.Failure<BaseResponseDto<TDto>>(result.ErrorMessage ?? "Failed to create entity.");
			}

			// If DTO carries connectors, delegate to connector writers
			if (@event.Payload is IConnectorsCarrier carrier && carrier.Connectors is { Count: > 0 })
			{
				foreach (var writer in connectorWriters)
				{
					await writer.WriteAsync(result.Value.Entity.Id, carrier.Connectors, cancellationToken);
				}
			}

			var dto = mapper.Map<TDto>(result.Value.Entity);

			// If DTO is a carrier, reload connectors via reader pipeline
			if (dto is IConnectorsCarrier dtoCarrier)
			{
				await LoadConnectorsForSingleEntityAsync(result.Value.Entity.Id, dtoCarrier, cancellationToken);
			}

			return Result.Success(new BaseResponseDto<TDto>(dto));
		}

		private async Task LoadConnectorsForSingleEntityAsync(Guid entityId, IConnectorsCarrier dtoCarrier, CancellationToken cancellationToken)
		{
			var readers = connectorWriters.OfType<IConnectorReader<TEntity>>(); // Trick: same DI scope; or inject separately
			// Better: inject IEnumerable<IConnectorReader<TEntity>> separately. For now, skip reader call in Create (writer already ran, no reload needed for POC).
			// Full implementation: inject readers, call LoadAsync([entityId]), merge.
			// Simplified POC: Leave Connectors empty after Create; client can GET to retrieve full state.
			dtoCarrier.Connectors = null; // or keep the incoming connectors as echo
		}
	}

	internal sealed class BaseGetByIdRequestDtoHandler<TEntity, TDto>(
		IMediator mediator,
		IMapper mapper,
		IEnumerable<IConnectorReader<TEntity>> connectorReaders)
		: IScopedHandler, IRequestHandler<BaseGetByIdRequestDto<TDto>, BaseResponseDto<TDto>>
		where TEntity : Entity
		where TDto : class
	{
		public async Task<IResult<BaseResponseDto<TDto>>> HandleAsync(BaseGetByIdRequestDto<TDto> @event, CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<GetEntityRequest<TEntity>, EntityResponse<TEntity>>(
				new GetEntityRequest<TEntity>(@event.Id),
				cancellationToken);

			if (!result.Succeeded || result.Value is null)
			{
				return Result.Failure<BaseResponseDto<TDto>>(result.ErrorMessage ?? "Entity not found.");
			}

			var dto = mapper.Map<TDto>(result.Value.Entity);

			// If DTO is a carrier, load connectors
			if (dto is IConnectorsCarrier carrier)
			{
				var connectorMap = new Dictionary<Guid, IReadOnlyList<BaseConnectorDto>>();
				foreach (var reader in connectorReaders)
				{
					var readerMap = await reader.LoadAsync(new[] { result.Value.Entity.Id }, cancellationToken);
					foreach (var kvp in readerMap)
					{
						if (connectorMap.ContainsKey(kvp.Key))
						{
							// Merge lists (multiple reader types)
							connectorMap[kvp.Key] = connectorMap[kvp.Key].Concat(kvp.Value).ToList();
						}
						else
						{
							connectorMap[kvp.Key] = kvp.Value;
						}
					}
				}

				carrier.Connectors = connectorMap.TryGetValue(result.Value.Entity.Id, out var connectors)
					? connectors
					: new List<BaseConnectorDto>();
			}

			return Result.Success(new BaseResponseDto<TDto>(dto));
		}
	}

	internal sealed class BaseGetAllRequestDtoHandler<TEntity, TDto>(
		IMediator mediator,
		IMapper mapper,
		IEnumerable<IConnectorReader<TEntity>> connectorReaders)
		: IScopedHandler, IRequestHandler<BaseGetAllRequestDto<TDto>, BaseListResponseDto<TDto>>
		where TEntity : Entity
		where TDto : class
	{
		public async Task<IResult<BaseListResponseDto<TDto>>> HandleAsync(BaseGetAllRequestDto<TDto> @event, CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<GetEntitiesRequest<TEntity>, EntityListResponse<TEntity>>(
				new GetEntitiesRequest<TEntity>(),
				cancellationToken);

			if (!result.Succeeded || result.Value is null)
			{
				return Result.Failure<BaseListResponseDto<TDto>>(result.ErrorMessage ?? "Failed to query entities.");
			}

			var dtos = mapper.Map<IReadOnlyList<TDto>>(result.Value.Entities);

			// If DTOs are carriers, batch-load connectors for all entities
			if (dtos.FirstOrDefault() is IConnectorsCarrier)
			{
				var entityIds = result.Value.Entities.Select(e => e.Id).ToList();
				var connectorMap = new Dictionary<Guid, IReadOnlyList<BaseConnectorDto>>();

				foreach (var reader in connectorReaders)
				{
					var readerMap = await reader.LoadAsync(entityIds, cancellationToken);
					foreach (var kvp in readerMap)
					{
						if (connectorMap.ContainsKey(kvp.Key))
						{
							// Merge lists (multiple reader types)
							connectorMap[kvp.Key] = connectorMap[kvp.Key].Concat(kvp.Value).ToList();
						}
						else
						{
							connectorMap[kvp.Key] = kvp.Value;
						}
					}
				}

				// Populate Connectors on each DTO
				foreach (var dto in dtos.OfType<IConnectorsCarrier>())
				{
					var entityId = (dto as dynamic).Id; // Fragile; better: introduce IIdentifiable<Guid> marker
					if (entityId is Guid id && connectorMap.TryGetValue(id, out var connectors))
					{
						dto.Connectors = connectors;
					}
					else
					{
						dto.Connectors = new List<BaseConnectorDto>();
					}
				}
			}

			return Result.Success(new BaseListResponseDto<TDto>(dtos));
		}
	}

	internal sealed class BaseUpdateRequestDtoHandler<TEntity, TDto>(
		IMediator mediator,
		IMapper mapper,
		IEnumerable<IConnectorWriter<TEntity>> connectorWriters,
		IEnumerable<IConnectorReader<TEntity>> connectorReaders)
		: IScopedHandler, IRequestHandler<BaseUpdateRequestDto<TDto>, BaseResponseDto<TDto>>
		where TEntity : Entity
		where TDto : class
	{
		public async Task<IResult<BaseResponseDto<TDto>>> HandleAsync(BaseUpdateRequestDto<TDto> @event, CancellationToken cancellationToken = default)
		{
			var entity = mapper.Map<TEntity>(@event.Payload);
			var result = await mediator.RequestAsync<UpdateEntityRequest<TEntity>, EntityResponse<TEntity>>(
				new UpdateEntityRequest<TEntity>(@event.Id, entity),
				cancellationToken);

			if (!result.Succeeded || result.Value is null)
			{
				return Result.Failure<BaseResponseDto<TDto>>(result.ErrorMessage ?? "Failed to update entity.");
			}

			// If DTO carries connectors, delegate to connector writers (replace semantics)
			if (@event.Payload is IConnectorsCarrier carrier && carrier.Connectors is not null)
			{
				foreach (var writer in connectorWriters)
				{
					await writer.WriteAsync(@event.Id, carrier.Connectors, cancellationToken);
				}
			}

			var dto = mapper.Map<TDto>(result.Value.Entity);

			// Reload connectors if DTO is a carrier
			if (dto is IConnectorsCarrier dtoCarrier)
			{
				var connectorMap = new Dictionary<Guid, IReadOnlyList<BaseConnectorDto>>();
				foreach (var reader in connectorReaders)
				{
					var readerMap = await reader.LoadAsync(new[] { @event.Id }, cancellationToken);
					foreach (var kvp in readerMap)
					{
						if (connectorMap.ContainsKey(kvp.Key))
						{
							connectorMap[kvp.Key] = connectorMap[kvp.Key].Concat(kvp.Value).ToList();
						}
						else
						{
							connectorMap[kvp.Key] = kvp.Value;
						}
					}
				}

				dtoCarrier.Connectors = connectorMap.TryGetValue(@event.Id, out var connectors)
					? connectors
					: new List<BaseConnectorDto>();
			}

			return Result.Success(new BaseResponseDto<TDto>(dto));
		}
	}

	internal sealed class BaseDeleteRequestDtoHandler<TEntity, TDto>(IMediator mediator)
		: IScopedHandler, IRequestHandler<BaseDeleteRequestDto<TDto>, BaseDeleteResponseDto>
		where TEntity : Entity
		where TDto : class
	{
		public async Task<IResult<BaseDeleteResponseDto>> HandleAsync(BaseDeleteRequestDto<TDto> @event, CancellationToken cancellationToken = default)
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
}
