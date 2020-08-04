using System;

namespace CrystalClear.HierarchySystem.Attributes
{
	/// <summary>
	///     Any class with this attribute is is not visible anywhere in the editor.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class HiddenHierarchyObjectAttribute : Attribute
	{
	}
}