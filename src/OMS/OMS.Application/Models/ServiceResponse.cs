using OMS.Common.Interfaces.Entity;

namespace OMS.Application.Models
{
    public record ServiceResponse<TEntity>(TEntity? Entity, IConnectorEntity[]? Connectors) where TEntity : IEntity<Guid>;
}
