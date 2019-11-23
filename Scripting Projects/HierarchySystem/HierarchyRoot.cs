using CrystalClear.HierarchySystem.Attributes;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	[HiddenHierarchyObject]
	public class HierarchyRoot : HierarchyObject
	{
#pragma warning disable IDE0051 // Remove unused private members
		private new bool IsRoot => true;

		public new HierarchyObject Parent => null;

		private new HierarchyObject Root => null;
#pragma warning restore IDE0051 // Remove unused private members

		public new string Name
		{
			get
			{
				return HierarchySystem.GetName(this);
			}
		}

		public Dictionary<string, HierarchyObject> HierarchyObjects => LocalHierarchy; // Refer HierarchyObjects to LocalHierarchy instead

	}
}
