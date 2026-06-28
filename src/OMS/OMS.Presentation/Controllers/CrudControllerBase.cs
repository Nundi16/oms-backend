using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Communication;
using OMS.Domain.Abstractions.Events;
using OMS.Domain.Interfaces.Events;

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
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync(CreationDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.RequestAsync<ICreationDomainEvent<TEntity>, TResponse>(request, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(ModificationDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.RequestAsync<IModificationDomainEvent<TEntity>, TResponse>(request, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(DeletionDomainEvent<TEntity> request, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.EmitAsync<IDeletionDomainEvent<TEntity>>(request, cancellationToken);
            return Ok();
        }
    }
}
