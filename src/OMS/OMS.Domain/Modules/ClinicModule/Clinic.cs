using OMS.Common.Abstractions.Entity;
using OMS.Common.Attributes;
using OMS.Common.Filter.Enums;

namespace OMS.Domain.Modules.ClinicModule
{
    public sealed class Clinic : Entity
    {
        [Filterable(nameof(Name), FilterOperator.Contains)]
        public string Name { get; set; }
    }
}
