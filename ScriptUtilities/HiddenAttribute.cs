using System;

namespace CrystalClear.HierarchySystem.Attributes
{
	/// <summary>
	///     Anything with this attribute is is not visible anywhere in the editor.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class HiddenAttribute : Attribute
	{
	}
}