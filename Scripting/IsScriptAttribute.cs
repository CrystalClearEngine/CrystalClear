using System;

namespace CrystalClear.ScriptUtilities // TODO: make this not required if already inheriting from HierarchyScript
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class IsScriptAttribute : Attribute
	{
	}
}