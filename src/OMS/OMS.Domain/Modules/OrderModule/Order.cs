using OMS.Common.Abstractions.Entity;

namespace OMS.Domain.Modules.OrderModule
{
    public sealed class Order : Entity
    {
        public string Name { get; set; }
    }
}
