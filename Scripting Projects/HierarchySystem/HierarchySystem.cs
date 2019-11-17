using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.HierarchySystem
{
	public static class HierarchySystem
	{
		public static HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			return LoadedHierarchies[pathSegments[0]].FollowPath(path.Remove(0, pathSegments[0].Length));
		}

		public static Dictionary<string, HierarchyRoot> LoadedHierarchies = new Dictionary<string, HierarchyRoot>();

		public static string GetName(HierarchyRoot hierarchyRoot) // TODO make this properly manage multiple values with the same type, maybe by not allowing duplicate values to be addes as one part of the solution
		{
			string key;
			key = LoadedHierarchies.FirstOrDefault(x => x.Value == hierarchyRoot).Key; // Get the key of this hierarchyRoot. Lets hope you dont have duplicate hierarchyRoots though... hmmm lets TODO that
			return key;
		}
	}
}
