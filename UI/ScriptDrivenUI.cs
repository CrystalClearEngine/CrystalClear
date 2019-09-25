using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
	public class Expose : Attribute { }

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
	public class Color : Attribute { }

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
	public class DebugInfo : Attribute { }
}
