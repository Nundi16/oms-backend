using Microsoft.AspNetCore.Mvc;
using OMS.Application.Communication.Requests;
using OMS.Application.Communication.Responses;
using OMS.Common.Interfaces.Communication;

namespace OMS.Presentation.Controllers
{
	[ApiController]
	public abstract class BaseCrudController<TDto>(IMediator mediator) : ControllerBase where TDto : class
	{
		[HttpPost]
		public virtual async Task<IActionResult> Create([FromBody] BaseCreateRequestDto<TDto> request, CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<BaseCreateRequestDto<TDto>, BaseResponseDto<TDto>>(request, cancellationToken);
			return result.Succeeded ? Ok(result.Value) : BadRequest(result.ErrorMessage);
		}

		[HttpGet("{id:guid}")]
		public virtual async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<BaseGetByIdRequestDto<TDto>, BaseResponseDto<TDto>>(new BaseGetByIdRequestDto<TDto>(id), cancellationToken);
			return result.Succeeded ? Ok(result.Value) : NotFound(result.ErrorMessage);
		}

		[HttpGet]
		public virtual async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<BaseGetAllRequestDto<TDto>, BaseListResponseDto<TDto>>(new BaseGetAllRequestDto<TDto>(), cancellationToken);
			return result.Succeeded ? Ok(result.Value) : BadRequest(result.ErrorMessage);
		}

		[HttpPut("{id:guid}")]
		public virtual async Task<IActionResult> Update(Guid id, [FromBody] BaseUpdateRequestDto<TDto> request, CancellationToken cancellationToken = default)
		{
			var normalized = request with { Id = id };
			var result = await mediator.RequestAsync<BaseUpdateRequestDto<TDto>, BaseResponseDto<TDto>>(normalized, cancellationToken);
			return result.Succeeded ? Ok(result.Value) : BadRequest(result.ErrorMessage);
		}

		[HttpDelete("{id:guid}")]
		public virtual async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<BaseDeleteRequestDto<TDto>, BaseDeleteResponseDto>(new BaseDeleteRequestDto<TDto>(id), cancellationToken);
			return result.Succeeded ? Ok(result.Value) : BadRequest(result.ErrorMessage);
		}

        public virtual async Task<IActionResult> GetFilters()
        {
            //TODO: Use DTO for filters
            return Ok(new { Filters = new List<string>() });
        }
    }
}
