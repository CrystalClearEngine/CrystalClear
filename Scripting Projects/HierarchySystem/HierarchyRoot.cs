using CrystalClear.HierarchySystem.Attributes;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	[HiddenHierarchyObject]
	public class HierarchyRoot : HierarchyObject
	{
		public HierarchyRoot() // Hmmm well I was gonna make a constructor with parameters and all, but I´ll settle for this as I don´t know what to add
		{

		}

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
