using Microsoft.AspNetCore.Mvc;
using OMS.Application.Common.Interfaces;
using OMS.Application.Communication.Requests;
using OMS.Application.Communication.Responses;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Communication;

namespace OMS.Presentation.Controllers
{
	[ApiController]
	public abstract class BaseCrudController<TEntity,TDto>(IMediator mediator) : ControllerBase where TEntity : Entity where TDto : IDto<TEntity>
	{
		[HttpPost]
		public virtual async Task<IActionResult> Create([FromBody] BaseCreateRequestDto<TEntity, TDto> request, CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<BaseCreateRequestDto<TEntity, TDto>, BaseResponseDto<TEntity>>(request, cancellationToken);
			return result.Succeeded ? Ok(result.Value) : BadRequest(result.ErrorMessage);
		}

		[HttpGet("{id:guid}")]
		public virtual async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<BaseGetByIdRequestDto<TEntity, TDto>, BaseResponseDto<TEntity>>(new BaseGetByIdRequestDto<TEntity, TDto>(id), cancellationToken);
			return result.Succeeded ? Ok(result.Value) : NotFound(result.ErrorMessage);
		}

		[HttpGet]
		public virtual async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<BaseGetAllRequestDto<TEntity, TDto>, BaseListResponseDto<TDto>>(new BaseGetAllRequestDto<TEntity, TDto>(), cancellationToken);
			return result.Succeeded ? Ok(result.Value) : BadRequest(result.ErrorMessage);
		}

		[HttpPut("{id:guid}")]
		public virtual async Task<IActionResult> Update(Guid id, [FromBody] BaseUpdateRequestDto<TEntity, TDto> request, CancellationToken cancellationToken = default)
		{
			request.Payload.Id = id;
			var result = await mediator.RequestAsync<BaseUpdateRequestDto<TEntity, TDto>, BaseResponseDto<TEntity>>(request, cancellationToken);
			return result.Succeeded ? Ok(result.Value) : BadRequest(result.ErrorMessage);
		}

		[HttpDelete("{id:guid}")]
		public virtual async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
		{
			var result = await mediator.RequestAsync<BaseDeleteRequestDto<TEntity, TDto>, BaseDeleteResponseDto>(new BaseDeleteRequestDto<TEntity, TDto>(id), cancellationToken);
			return result.Succeeded ? Ok(result.Value) : BadRequest(result.ErrorMessage);
		}

        public virtual async Task<IActionResult> GetFilters()
        {
            //TODO: Use DTO for filters
            return Ok(new { Filters = new List<string>() });
        }
    }
}
