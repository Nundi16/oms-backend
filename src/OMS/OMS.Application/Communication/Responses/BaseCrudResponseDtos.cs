using OMS.Application.Common.Interfaces;
using OMS.Application.Models;
using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Communication.Responses
{
	public sealed record BaseResponseDto<TEntity>(IDto<TEntity> Payload) where TEntity : Entity;

	public sealed record BaseListResponseDto<TDto>(IReadOnlyList<TDto> Payload) where TDto : IDto;

	public sealed record BaseDeleteResponseDto(Guid Id, bool Success);
    public sealed record BaseFilterResponseDto(IList<FilterTypeDto> Filters);
}
