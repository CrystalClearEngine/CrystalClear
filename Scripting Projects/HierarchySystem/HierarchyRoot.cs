using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	public class HierarchyRoot
	{
		public string Name
		{
			get
			{
				return HierarchySystem.GetName(this);
			}
		}

		public Dictionary<string, HierarchyObject> HierarchyObjects = new Dictionary<string, HierarchyObject>();

		public HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			return HierarchyObjects[pathSegments[0]].FollowPath(path.Remove(0, pathSegments[0].Length));
		}
	}
}
