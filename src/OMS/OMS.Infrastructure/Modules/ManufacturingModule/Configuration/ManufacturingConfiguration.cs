using OMS.Domain.Modules.ClinicModule;
using OMS.Domain.Modules.ManufacturingModule;
using OMS.Infrastructure.Abstractions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Infrastructure.Modules.ManufacturingModule.Configuration
{
	internal sealed class SiteConfiguration : EntityConfiguration<Site>;
	internal sealed class MachineConfiguration : EntityConfiguration<Machine>;

}
