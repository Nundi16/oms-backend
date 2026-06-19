using OMS.Common.Abstractions.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OMS.Domain.Modules.ManufacturingModule
{
	public class Machine : Entity
	{
		public string Name { get; set; }

		public virtual Site? Site { get; set; }
	}
}
