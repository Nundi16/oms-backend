using Microsoft.AspNetCore.Mvc;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Abstractions.Events;

namespace OMS.Presentation.Controllers
{
    public abstract class CrudControllerBase<TEntity, TResponse>(IMediator mediator) : ControllerBase
        where TEntity : Entity
        where TResponse : class
    {
        protected IMediator Mediator = mediator;

        [HttpGet("{id:guid:required}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CancellationToken cancellationToken = default)
        {
            // main entity, entity.connector
            // [ entity, entity.connector ].Select(x => x.toDomainEvent())
            // handler.RequestAll([])
            // done
            var result = Mediator.RequestAsync<CreationDomainEvent<TEntity>, TResponse>(cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(CancellationToken cancellationToken = default)
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(CancellationToken cancellationToken = default)
        {
            return Ok();
        }
    }
}
