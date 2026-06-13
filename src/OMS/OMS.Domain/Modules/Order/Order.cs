using OMS.Common.Abstractions.Entity;

namespace OMS.Domain.Modules
{
    public sealed class Order : Entity
    {
        public string Name { get; set; }
    }
}
