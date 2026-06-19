using System;
using System.Collections.Generic;
using System.Text;

using OMS.Application.Common.Interfaces;
using OMS.Domain.Modules.ManufacturingModule;

namespace OMS.Application.Modules.ManufacturingModule.Models
{
	public sealed class SiteDto(Guid? id, string name) : IDto<Site>
	{
		public Guid? Id { get; set; } = id;
		public string Name { get; set; } = name ?? string.Empty;
	}
}
