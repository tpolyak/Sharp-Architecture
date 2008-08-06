using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBase.Core
{
	public class ConcreteTypeAttribute : Attribute
	{
		public string TypeName { get; set; }

		public ConcreteTypeAttribute(string typeName) {
			this.TypeName = typeName;
		}
	}
}
