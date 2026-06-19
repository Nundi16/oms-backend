using System;
using System.Collections.Generic;
using System.Text;

using OMS.Application.Common.Interfaces;
using OMS.Domain.Modules.ManufacturingModule;

namespace OMS.Application.Modules.ManufacturingModule.Models
{
	public sealed class MachineDto(Guid? id, Guid? siteId, string name) : IDto<Machine>
	{
		public Guid? Id { get; set; } = id;
		public Guid? SiteId { get; set; } = siteId;
		public string Name { get; set; } = name ?? string.Empty;
	}
}
