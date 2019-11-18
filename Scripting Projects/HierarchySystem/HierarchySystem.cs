using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.HierarchySystem
{
	public class HierarchySystem
	{
		public static HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			string pathToFollow = path.Remove(0, pathSegments[0].Length + 1);
			string nextObject = pathSegments[1];
			return LoadedHierarchies[nextObject].FollowPath(pathToFollow);
		}

		public static Dictionary<string, HierarchyObject> LoadedHierarchies = new Dictionary<string, HierarchyObject>();

		public static void SetName(HierarchyObject hierarchyObject, string newName) // TODO below TODO
		{
			string key = LoadedHierarchies.FirstOrDefault(x => x.Value == hierarchyObject).Key;
			LoadedHierarchies.Remove(key);
			LoadedHierarchies.Add(newName, hierarchyObject);
		}

		public static string GetName(HierarchyObject hierarchyObject) // TODO make this properly manage multiple values with the same type, maybe by not allowing duplicate values to be addes as one part of the solution
		{
			string key;
			key = LoadedHierarchies.FirstOrDefault(x => x.Value == hierarchyObject).Key; // Get the key of this hierarchyRoot. Lets hope you dont have duplicate hierarchyRoots though... hmmm lets TODO that
			return key;
		}
	}
}
