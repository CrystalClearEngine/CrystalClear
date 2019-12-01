using CrystalClear.HierarchySystem.Attributes;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// HierarchyRoot is a custom HierarchyObject type specifically designed to be at the root of a regular Hierarchy.
	/// </summary>
	[HiddenHierarchyObject]
	public class HierarchyRoot : HierarchyObject
	{
#pragma warning disable IDE0051 // Remove unused private members
		// This has to be true, since a HierarchyRoot has to be at the root of the Hierarchy.
		private new bool IsRoot => true;

		// There can be no parent to a HierarchyRoot, as it is always at the root of the Hierarchy.
		public new HierarchyObject Parent => null;

		// This HierarchyObject has to be the root.
		private new HierarchyObject Root
		{
			get => this;
		}
#pragma warning restore IDE0051 // Remove unused private members

		// The name of a HierarchyRoot is stored in the HierarchySystem in LoadedHierarchies.
		public new string Name
		{
			get
			{
				return HierarchySystem.GetHierarchyName(this);
			}
		}

		// Refer HierarchyObjects to LocalHierarchy instead.
		public Dictionary<string, HierarchyObject> HierarchyObjects => LocalHierarchy;

	}
}
