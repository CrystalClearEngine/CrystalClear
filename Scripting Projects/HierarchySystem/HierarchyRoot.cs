using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	public class HierarchyRoot : HierarchyObject/* : IHierarchyObjectManager*/
	{
		public new string Name
		{
			get
			{
				return HierarchySystem.GetName(this);
			}
		}

		public Dictionary<string, HierarchyObject> HierarchyObjects = new Dictionary<string, HierarchyObject>();

		public new Dictionary<string, HierarchyObject> LocalHierarchy
		{
			get => HierarchyObjects;
			set
			{
				HierarchyObjects = LocalHierarchy;
			}
		}

		public new HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			return HierarchyObjects[pathSegments[1]].FollowPath(path.Remove(0, pathSegments[0].Length));
		}
	}
}
