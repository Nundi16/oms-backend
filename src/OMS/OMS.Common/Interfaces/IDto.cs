using OMS.Common.Abstractions.Entity;

namespace OMS.Application.Common.Interfaces
{
	public interface IDto
	{
	}

	public interface IDto<TEntity> : IDto where TEntity : Entity
	{
		Guid? Id { get; set; }
	}
}