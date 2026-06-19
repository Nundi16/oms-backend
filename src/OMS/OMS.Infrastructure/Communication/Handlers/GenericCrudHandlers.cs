using Microsoft.EntityFrameworkCore;
using OMS.Application.Communication.Requests;
using OMS.Application.Communication.Responses;
using OMS.Application.Connectors.Pipeline;
using OMS.Application.Interfaces.Communication;
using OMS.Common;
using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces;
using OMS.Common.Interfaces.Communication;
using OMS.Common.Interfaces.Communication.Handlers;
using OMS.Common.Interfaces.Communication.Handlers.Request;
using OMS.Domain.Abstractions.Events;

namespace OMS.Infrastructure.Communication.Handlers
{
	/// <summary>
	/// Generic handler for creating entities
	/// This handler will be used unless a more specific implementation exists
	/// </summary>
	/// <typeparam name="TEntity">The entity type to create</typeparam>
	internal class CreateEntityRequestHandler<TEntity>(
		IInfrastructureMediator infrastructureMediator,
		ApplicationDbContext dbContext) 
		: IScopedHandler, IRequestHandler<CreateEntityRequest<TEntity>, EntityResponse<TEntity>>
		where TEntity : Entity
	{
		public async Task<IResult<EntityResponse<TEntity>>> HandleAsync(
			CreateEntityRequest<TEntity> request, 
			CancellationToken cancellationToken = default)
		{
			if (request?.Entity == null)
			{
				return Result.Failure<EntityResponse<TEntity>>("Entity cannot be null");
			}

			// Use the infrastructure mediator to handle the creation with domain events
			var creationEvent = new CreationDomainEvent<TEntity>(request.Entity);
			var entity = await infrastructureMediator.HandleCreationAsync<CreationDomainEvent<TEntity>, TEntity>(
				creationEvent, 
				cancellationToken);

			// Save changes to persist the entity
			await dbContext.SaveChangesAsync(cancellationToken);

			var response = new EntityResponse<TEntity>(entity);
			return Result.Success(response);
		}
	}

	/// <summary>
	/// Generic handler for updating entities
	/// This handler will be used unless a more specific implementation exists
	/// </summary>
	/// <typeparam name="TEntity">The entity type to update</typeparam>
	internal class UpdateEntityRequestHandler<TEntity>(
		IInfrastructureMediator infrastructureMediator,
		ApplicationDbContext dbContext) 
		: IScopedHandler, IRequestHandler<UpdateEntityRequest<TEntity>, EntityResponse<TEntity>>
		where TEntity : Entity
	{
		public async Task<IResult<EntityResponse<TEntity>>> HandleAsync(
			UpdateEntityRequest<TEntity> request, 
			CancellationToken cancellationToken = default)
		{
			if (request?.Entity == null)
			{
				return Result.Failure<EntityResponse<TEntity>>("Entity cannot be null");
			}

			// Find the existing entity
			var dbSet = dbContext.Set<TEntity>();
			var existingEntity = await dbSet.FindAsync([request.Id], cancellationToken);

			if (existingEntity == null)
			{
				return Result.Failure<EntityResponse<TEntity>>($"Entity with ID {request.Id} not found");
			}
			// Update the entity using the infrastructure mediator
			var modificationEvent = new ModificationDomainEvent<TEntity>(request.Entity);
			var updatedEntity = infrastructureMediator.HandleModification<ModificationDomainEvent<TEntity>, TEntity>(
				modificationEvent);

			// Save changes
			await dbContext.SaveChangesAsync(cancellationToken);

			var response = new EntityResponse<TEntity>(updatedEntity);
			return Result.Success(response);
		}
	}

	/// <summary>
	/// Generic handler for deleting entities
	/// This handler will be used unless a more specific implementation exists
	/// </summary>
	/// <typeparam name="TEntity">The entity type to delete</typeparam>
	internal class DeleteEntityRequestHandler<TEntity>(
		IInfrastructureMediator infrastructureMediator,
		ApplicationDbContext dbContext) 
		: IScopedHandler, IRequestHandler<DeleteEntityRequest<TEntity>, DeleteEntityResponse>
		where TEntity : Entity
	{
		public async Task<IResult<DeleteEntityResponse>> HandleAsync(
			DeleteEntityRequest<TEntity> request, 
			CancellationToken cancellationToken = default)
		{
			// Find the existing entity
			var dbSet = dbContext.Set<TEntity>();
			var existingEntity = await dbSet.FindAsync([request.Id], cancellationToken);

			if (existingEntity == null)
			{
				return Result.Failure<DeleteEntityResponse>($"Entity with ID {request.Id} not found");
			}

			// Delete the entity using the infrastructure mediator
			var deletionEvent = new DeletionDomainEvent<TEntity>(existingEntity);
			infrastructureMediator.HandleDeletion<DeletionDomainEvent<TEntity>, TEntity>(deletionEvent);

			// Save changes
			await dbContext.SaveChangesAsync(cancellationToken);

			var response = new DeleteEntityResponse(request.Id, Success: true);
			return Result.Success(response);
		}
	}

	/// <summary>
	/// Generic handler for getting a single entity by ID.
	/// Composes connector-supplied LINQ filters by fanning out an <see cref="EntityQueryContext{TEntity}"/>
	/// through the mediator. Each authorized connector handler may add deferred WHERE/JOIN
	/// expressions to the shared query before it is materialised here.
	/// </summary>
	/// <typeparam name="TEntity">The entity type to retrieve</typeparam>
	internal class GetEntityRequestHandler<TEntity>(
		ApplicationDbContext dbContext,
		IMediator mediator)
		: IScopedHandler, IRequestHandler<GetEntityRequest<TEntity>, EntityResponse<TEntity>>
		where TEntity : Entity
	{
		public async Task<IResult<EntityResponse<TEntity>>> HandleAsync(
			GetEntityRequest<TEntity> request, 
			CancellationToken cancellationToken = default)
		{
			var ctx = new EntityQueryContext<TEntity>(dbContext.Set<TEntity>());
			await mediator.FanOutSequentialAsync(ctx, cancellationToken);

			// Materialise after all authorized handlers have composed their filters.
			var entity = await ctx.Query.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

			if (entity == null)
			{
				return Result.Failure<EntityResponse<TEntity>>($"Entity with ID {request.Id} not found or filtered out");
			}

			var response = new EntityResponse<TEntity>(entity);
			return Result.Success(response);
		}
	}

	/// <summary>
	/// Generic handler for getting a list of entities.
	/// Composes connector-supplied LINQ filters by fanning out an <see cref="EntityQueryContext{TEntity}"/>
	/// through the mediator before materialising the result.
	/// TODO: Add pagination, sorting support.
	/// </summary>
	/// <typeparam name="TEntity">The entity type to retrieve</typeparam>
	internal class GetEntitiesRequestHandler<TEntity>(
		ApplicationDbContext dbContext,
		IMediator mediator)
		: IScopedHandler, IRequestHandler<GetEntitiesRequest<TEntity>, EntityListResponse<TEntity>>
		where TEntity : Entity
	{
		public async Task<IResult<EntityListResponse<TEntity>>> HandleAsync(
			GetEntitiesRequest<TEntity> request, 
			CancellationToken cancellationToken = default)
		{
			var ctx = new EntityQueryContext<TEntity>(dbContext.Set<TEntity>());
			await mediator.FanOutSequentialAsync(ctx, cancellationToken);

			var entities = await ctx.Query.ToListAsync(cancellationToken);

			var response = new EntityListResponse<TEntity>(entities);
			return Result.Success(response);
		}
	}
}
