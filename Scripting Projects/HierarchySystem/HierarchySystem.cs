using System;
using System.Collections.Generic;
using System.Linq;

namespace CrystalClear.HierarchySystem
{
	public static class HierarchySystem
	{
		/// <summary>
		/// Follows a path and returns the HierarchyObject at the end.
		/// </summary>
		/// <param name="path">The path to follow. For instance: "UI/HUD/HealthSld/Text" will return the "Text" HierarchyObject, which is a child of "HealthSld" which is a child of "HUD" and so on</param>
		/// <returns>The HierarchyObject at the end of the path</returns>
		public static HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');
			string pathToFollow = path;
			string nextObject = pathSegments[0];
			return LoadedHierarchies[nextObject].FollowPath(pathToFollow);
		}

		/// <summary>
		/// The dictionary containing all currently loaded hierarchies.
		/// </summary>
		public static Dictionary<string, HierarchyObject> LoadedHierarchies = new Dictionary<string, HierarchyObject>();

		/// <summary>
		/// Adds a Hierarchy to the loaded hierarchies. Use LoadHierarchy() unless the Hierarchy is constructed at runtime.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="hierarchyObject"></param>
		public static void AddHierarchy(string name, HierarchyObject hierarchyObject)
		{
			LoadedHierarchies.Add(name, hierarchyObject);
		}

		public static void SetHierarchyName(HierarchyObject hierarchyObject, string newName)
		{
			string key = GetHierarchyName(hierarchyObject);
			LoadedHierarchies.Remove(key);
			LoadedHierarchies.Add(newName, hierarchyObject);
		}

		public static void SetHierarchyName(string currentName, string newName)
		{
			HierarchyObject hierarchy = LoadedHierarchies[currentName];
			LoadedHierarchies.Remove(currentName);
			LoadedHierarchies.Add(newName, hierarchy);
		}

		public static string GetHierarchyName(HierarchyObject hierarchyObject)
		{
			string key = LoadedHierarchies.FirstOrDefault(x => ReferenceEquals(x.Value, hierarchyObject)).Key;
			return key;
		}
	}
}
