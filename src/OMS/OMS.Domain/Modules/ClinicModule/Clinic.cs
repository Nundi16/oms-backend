using OMS.Common.Abstractions.Entity;
using OMS.Domain.Attributes;

namespace OMS.Domain.Modules.ClinicModule
{
    public sealed class Clinic : Entity
    {
        [Filterable(Roles.Clinic)]
        public string Name { get; set; }
    }
}
