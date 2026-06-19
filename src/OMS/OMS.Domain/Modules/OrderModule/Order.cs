using OMS.Common.Abstractions.Entity;
using OMS.Domain.Attributes;

namespace OMS.Domain.Modules.OrderModule
{
    public sealed class Order : Entity
    {
        [Filterable(Roles.Order)]
        public string Name { get; set; }
    }
}
