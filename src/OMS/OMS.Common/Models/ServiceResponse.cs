using OMS.Common.Interfaces.Entity;
using OMS.Common.Interfaces.Connectors;

namespace OMS.Common.Models
{
    public record ServiceResponse<TEntity>(TEntity? Entity, IConnector[] Connectors) where TEntity : IEntity<Guid>;
}
