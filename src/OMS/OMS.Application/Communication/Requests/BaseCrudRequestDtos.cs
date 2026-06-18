namespace OMS.Application.Communication.Requests
{
	public sealed record BaseCreateRequestDto<TDto>(TDto Payload) where TDto : class;

	public sealed record BaseGetByIdRequestDto<TDto>(Guid Id) where TDto : class;

	public sealed record BaseGetAllRequestDto<TDto>() where TDto : class;

	public sealed record BaseUpdateRequestDto<TDto>(Guid Id, TDto Payload) where TDto : class;

	public sealed record BaseDeleteRequestDto<TDto>(Guid Id) where TDto : class;
}
