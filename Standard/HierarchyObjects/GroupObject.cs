using System.Collections.Generic;
using CrystalClear.HierarchySystem;

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