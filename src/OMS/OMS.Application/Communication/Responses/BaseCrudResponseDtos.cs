namespace OMS.Application.Communication.Responses
{
	public sealed record BaseResponseDto<TDto>(TDto Payload) where TDto : class;

	public sealed record BaseListResponseDto<TDto>(IReadOnlyList<TDto> Payload) where TDto : class;

	public sealed record BaseDeleteResponseDto(Guid Id, bool Success);
}
