using System;

namespace CrystalClear.HierarchySystem.Attributes
{
	/// <summary>
	/// Any HierarchyObject class with this attribute is treated as special and is not visible in the editor
	/// </summary>
	/// 
	[AttributeUsage(AttributeTargets.Class)]
	public class HiddenHierarchyObjectAttribute : Attribute
	{
	}
}