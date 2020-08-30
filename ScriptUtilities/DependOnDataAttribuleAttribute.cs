using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.ScriptUtilities
{
	public sealed class DependOnDataAttributeAttribute : Attribute
	{
		public Type DataAttributeType { get; }

		public DependOnDataAttributeAttribute(Type dataAttributeType)
		{
			DataAttributeType = dataAttributeType;
		}
	}
}
