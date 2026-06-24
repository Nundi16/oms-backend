using OMS.Common.Abstractions.Entity;
using OMS.Common.Interfaces.Entity;

namespace OMS.Application.Models
{
    public abstract record ServiceResponse<TEntity> where TEntity : IEntity<Guid>;
}
