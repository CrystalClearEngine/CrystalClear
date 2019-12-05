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
			// Split the path into segments separated by /.
			string[] pathSegments = path.Split('/');
			// Store the path to send to the next object to follow.
			string pathToFollow = path;
			// Store the next object to go to.
			string nextObject = pathSegments[0];
			// Return the result of following the next object and passing it the path.
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
			// Add the HierarchyObject.
			LoadedHierarchies.Add(name, hierarchyObject);
		}

		/// <summary>
		/// Renames a loaded Hierarchy by instance.
		/// </summary>
		/// <param name="hierarchyObject">The HierarchyObject in LoadedHierarchies to rename.</param>
		/// <param name="newName">The new name to use.</param>
		public static void SetHierarchyName(HierarchyObject hierarchyObject, string newName)
		{
			// Get the current name.
			string key = GetHierarchyName(hierarchyObject);
			// Remove the HierarchyObject from LoadedHierarchies using it's old name.
			LoadedHierarchies.Remove(key);
			// Add the HierarchyObject back with it's new name.
			LoadedHierarchies.Add(newName, hierarchyObject);
		}

		/// <summary>
		/// Renames a loaded Hierarchy by key.
		/// </summary>
		/// <param name="currentName">The current name of the Hierarchy.</param>
		/// <param name="newName">The new name of the Hierarchy.</param>
		public static void SetHierarchyName(string currentName, string newName)
		{
			// Get the HierarchyObject that will be renamed.
			HierarchyObject hierarchy = LoadedHierarchies[currentName];
			// Remove the Hierarchy from the LoadedHierarchies using it's old name.
			LoadedHierarchies.Remove(currentName);
			// Add the Hierarchy back with it's new name.
			LoadedHierarchies.Add(newName, hierarchy);
		}

		/// <summary>
		/// Get the key (name) of a Hierarchy in LoadedHierarchies.
		/// </summary>
		/// <param name="hierarchy">The Hierarchy to find the name of.</param>
		/// <returns>The name of the HierarchyObject.</returns>
		public static string GetHierarchyName(HierarchyObject hierarchy)
		{
			// Define a string named key, to be used for storing the key.
			string key;
			// Set key.
			key = LoadedHierarchies
				.First( // Get the first occurance of...
					x => // X is the KeyValue pair of the dictionary.
					ReferenceEquals(x.Value, hierarchy)) // If the reference (x's value)'s instance equals the instance of the Hierarchy, then this is the Hierarchy we are searching for.
							.Key; // Get the item's key.
								  // Return the key.
			return key;
		}
	}
}
