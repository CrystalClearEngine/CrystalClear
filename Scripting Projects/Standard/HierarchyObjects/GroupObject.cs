using CrystalClear.HierarchySystem;
using System.Collections.Generic;

namespace CrystalClear.Standard.HierarchyObjects
{
	public class GroupObject : HierarchyObject
	{
		public Dictionary<string, HierarchyObject> Members;
	}

	public class GroupObject<TGroup> : HierarchyObject
		where TGroup : HierarchyObject
	{
		public Dictionary<string, TGroup> Members;
	}
}
