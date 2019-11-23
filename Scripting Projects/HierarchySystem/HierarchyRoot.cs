using CrystalClear.HierarchySystem.Attributes;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	[HiddenHierarchyObject]
	public class HierarchyRoot : HierarchyObject
	{
		private readonly new bool IsRoot = true;

		private readonly new HierarchyObject Parent = null;

		private readonly new HierarchyObject Root = null;

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
