using OMS.Common.Interfaces.Entity;
using OMS.Common.Interfaces.Connectors;
using OMS.Common.Interfaces.Extensions;

namespace OMS.Common.Models
{
    public record ServiceResponse<TEntity>(TEntity? Entity, IConnector[] Connectors, IExtension[] Extensions) where TEntity : IEntity<Guid>;
}
