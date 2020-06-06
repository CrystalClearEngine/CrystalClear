using System;
using System.Linq;
using System.Reflection;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObjectProperties
	{
		// Analyzing.
		/// <summary>
		/// Finds all types that derive from HierarchyObject in the assembly and returns them.
		/// </summary>
		/// <param name="assembly">The assembly to search for HierarchyObject types in.</param>
		/// <returns>The found HierarchyObjects.</returns>
		public static Type[] FindHierarchyObjectTypesInAssembly(Assembly assembly) => FindHierarchyObjectTypesInTypes(assembly.GetTypes());

		/// <summary>
		/// Finds all types that derive from HierarchyObject in the type array and returns them.
		/// </summary>
		/// <param name="types">The types to search for HierarchyObject types in.</param>
		/// <returns>The found HierarchyObjects.</returns>
		public static Type[] FindHierarchyObjectTypesInTypes(params Type[] types)
		{
			// Find and store the found HierarchyObject types.
			Type[] customHierarchyObjects = (from type in types // Iterator variable.
											 where IsHierarchyObject(type) // Is the type a HierarchyObject?
											 select type).ToArray();
			// Return the found HierarchyObjects.
			return customHierarchyObjects;
		}

		/// <summary>
		/// Checks whether the provided type derives from HierarchyObject.
		/// </summary>
		/// <param name="type">The type to check whether it derives from HierarchyObject.</param>
		/// <returns>"ether the provided type derives from HierarchyObject.</returns>
		public static bool IsHierarchyObject(Type type)
		{
			return type.IsSubclassOf(typeof(HierarchyObjectProperties));
		}
	}
}
