using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.HierarchySystem
{
	public /*static*/ class HierarchySystem/* : IHierarchyObjectManager*/
	{
		static HierarchySystem() // Just to make sure we´re clear - HierarchySystem is effectively a static class, just not marked as it so it can implement //IHierarchyObjectManager
		{
		}

		public static HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			string pathToFollow = path.Remove(0, pathSegments[0].Length + 1);
			string nextObject = pathSegments[1];
			return LoadedHierarchies[nextObject].FollowPath(pathToFollow);
		}

		public static Dictionary<string, HierarchyRoot> LoadedHierarchies = new Dictionary<string, HierarchyRoot>();

		private void SetName(HierarchyRoot hierarchyRoot)
		{
			string key = LoadedHierarchies.FirstOrDefault(x => x.Value == hierarchyRoot).Key;
			LoadedHierarchies.Remove(key);
			LoadedHierarchies.Add(key, hierarchyRoot);
		}

		public static string GetName(HierarchyRoot hierarchyRoot) // TODO make this properly manage multiple values with the same type, maybe by not allowing duplicate values to be addes as one part of the solution
		{
			string key;
			key = LoadedHierarchies.FirstOrDefault(x => x.Value == hierarchyRoot).Key; // Get the key of this hierarchyRoot. Lets hope you dont have duplicate hierarchyRoots though... hmmm lets TODO that
			return key;
		}
	}
}
