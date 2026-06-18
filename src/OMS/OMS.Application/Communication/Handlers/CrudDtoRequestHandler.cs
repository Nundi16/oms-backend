using AutoMapper;
using OMS.Application.Communication.Requests;
using OMS.Application.Communication.Responses;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers;
using OMS.Common.Interfaces.Communication.Handlers.Request;

namespace OMS.Application.Communication.Handlers
{
	internal sealed class BaseCreateRequestDtoHandler<TEntity, TDto>(IMediator mediator, IMapper mapper)
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

			var dto = mapper.Map<TDto>(result.Value.Entity);
			return Result.Success(new BaseResponseDto<TDto>(dto));
		}
	}

	internal sealed class BaseGetByIdRequestDtoHandler<TEntity, TDto>(IMediator mediator, IMapper mapper)
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
			return Result.Success(new BaseResponseDto<TDto>(dto));
		}
	}

	internal sealed class BaseGetAllRequestDtoHandler<TEntity, TDto>(IMediator mediator, IMapper mapper)
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
			return Result.Success(new BaseListResponseDto<TDto>(dtos));
		}
	}

	internal sealed class BaseUpdateRequestDtoHandler<TEntity, TDto>(IMediator mediator, IMapper mapper)
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

			var dto = mapper.Map<TDto>(result.Value.Entity);
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
