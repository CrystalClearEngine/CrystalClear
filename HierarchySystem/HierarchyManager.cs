﻿#define Editor
using CrystalClear.EventSystem.StandardEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrystalClear.HierarchySystem
{
	public static class HierarchyManager
	{
		/// <summary>
		///     The dictionary containing all currently loaded Hierarchies.
		/// </summary>
		public static Dictionary<string, HierarchyObject> LoadedHierarchies = new Dictionary<string, HierarchyObject>();

		public static event Action HierarchiesChanged;

		/// <summary>
		///     Follows a path and returns the HierarchyObject at the end.
		/// </summary>
		/// <param name="path">
		///     The path to follow. For instance: "UI/HUD/HealthSld/Text" will return the "Text" HierarchyObject,
		///     which is a child of "HealthSld" which is a child of "HUD" and so on
		/// </param>
		/// <returns>The HierarchyObject at the end of the path</returns>
		public static HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/');

			var pathToFollow = path;

			var nextObject = pathSegments[0];

			return LoadedHierarchies[nextObject].FollowPath(pathToFollow);
		}

		/// <summary>
		///     Adds a Hierarchy to the loaded Hierarchies. Use LoadHierarchy() unless the Hierarchy is constructed at runtime.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="hierarchyObject"></param>
		public static void AddHierarchy(string name, HierarchyObject hierarchyObject)
		{
			LoadedHierarchies.Add(name, hierarchyObject);

			HierarchiesChanged?.Invoke();
		}

		/// <summary>
		///     Renames a loaded Hierarchy by instance.
		/// </summary>
		/// <param name="hierarchyObject">The HierarchyObject in LoadedHierarchies to rename.</param>
		/// <param name="newName">The new name to use.</param>
		public static void SetHierarchyName(HierarchyObject hierarchyObject, string newName)
		{
			HierarchiesChanged?.Invoke();

			var key = GetHierarchyName(hierarchyObject);

			LoadedHierarchies.Remove(key);

			LoadedHierarchies.Add(newName, hierarchyObject);
		}

		/// <summary>
		///     Renames a loaded Hierarchy by key.
		/// </summary>
		/// <param name="currentName">The current name of the Hierarchy.</param>
		/// <param name="newName">The new name of the Hierarchy.</param>
		public static void SetHierarchyName(string currentName, string newName)
		{
			HierarchiesChanged?.Invoke();

			HierarchyObject hierarchy = LoadedHierarchies[currentName];

			LoadedHierarchies.Remove(currentName);

			LoadedHierarchies.Add(newName, hierarchy);
		}

		/// <summary>
		///     Get the key (name) of a Hierarchy in LoadedHierarchies.
		/// </summary>
		/// <param name="hierarchyObject">The Hierarchy to find the name of.</param>
		/// <returns>The name of the HierarchyObject.</returns>
		public static string GetHierarchyName(HierarchyObject hierarchyObject)
		{
			if (!LoadedHierarchies.ContainsValue(hierarchyObject))
			{
				throw new KeyNotFoundException("Hierarchy not found!");
			}

			string key;

			key = LoadedHierarchies.First(x => ReferenceEquals(x.Value, hierarchyObject)).Key;

			return key;
		}

#if Editor
		[OnStopEvent]
		public static void EditorReset()
		{
			LoadedHierarchies.Clear();
		}
#endif
	}
}