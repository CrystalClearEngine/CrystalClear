using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.ScriptUtilities
{
	// TODO: use this attribute!
	public sealed class DependOnDataAttributeAttribute : Attribute
	{
		public Type DataAttributeType { get; }

		public DependOnDataAttributeAttribute(Type dataAttributeType)
		{
			DataAttributeType = dataAttributeType;
		}
	}
}
