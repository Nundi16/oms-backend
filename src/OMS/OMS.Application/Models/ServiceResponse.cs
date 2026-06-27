using OMS.Common.Interfaces.Entity;
using OMS.Domain.Interfaces.Connectors;

namespace OMS.Application.Models
{
    public record ServiceResponse<TEntity>(TEntity? Entity, IConnector[] Connectors) where TEntity : IEntity<Guid>;
}
