using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.HierarchySystem
{
	public class HierarchyRoot
	{
		public string Name
		{
			get;
			private set;
		}

		public Dictionary<string, HierarchyObject> HierarchyObjects = new Dictionary<string, HierarchyObject>();

		public HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			if (pathSegments.Length == 0)
			{
				throw new Exception(pathSegments[0] + " is not a HierarchyObject");
			}
			return HierarchyObjects[pathSegments[0]].FollowPath(path.Remove(0, pathSegments[0].Length));
		}
	}

	public static class HierarchySystem
	{
		public static HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			if (pathSegments.Length == 0)
			{
				throw new Exception(pathSegments[0] + " is not a HierarchyObject");
			}
			return LoadedHierarchies[pathSegments[0]].FollowPath(path.Remove(0, pathSegments[0].Length));
		}

		public static Dictionary<string, HierarchyRoot> LoadedHierarchies = new Dictionary<string, HierarchyRoot>();
	}
}
