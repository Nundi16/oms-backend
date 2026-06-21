using OMS.Application.Common.Interfaces;
using OMS.Application.Models;
using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Communication.Requests
{
	public sealed record BaseCreateRequestDto<TEntity,TDto>(TDto Payload) where TEntity : Entity where TDto : IDto<TEntity>;

	public sealed record BaseGetByIdRequestDto<TEntity,TDto>(Guid Id) where TEntity : Entity where TDto : IDto<TEntity>;

	public sealed record BaseGetAllRequestDto<TEntity,TDto>(IReadOnlyList<FilterDto> Filters) where TEntity : Entity where TDto : IDto<TEntity>;

	public sealed record BaseUpdateRequestDto<TEntity,TDto>(TDto Payload) where TEntity : Entity where TDto : IDto<TEntity>;

	public sealed record BaseDeleteRequestDto<TEntity,TDto>(Guid Id) where TEntity : Entity where TDto : IDto<TEntity>;

	public sealed record BaseFilterRequestDto<TEntity>() where TEntity : Entity;
}
