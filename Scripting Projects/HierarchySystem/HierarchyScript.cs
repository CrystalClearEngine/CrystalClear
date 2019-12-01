using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalClear.HierarchySystem;
using System.Reflection;

namespace CrystalClear.HierarchySystem.Scripting
{
	/// <summary>
	/// The type that all HierarchyScripts derive from. This is like a scripting "interface" for scripts to act through.
	/// </summary>
	/// <typeparam name="T">The type of HierarhcyObjects that this HierarchyScript will be targeting.</typeparam>
	public abstract class HierarchyScript<T>
	{
		public void SetUp(T hierarchyObject)
		{
			HierarchyObject = hierarchyObject;
		}

		public T HierarchyObject;
	}

	internal static class HierarchyScript
	{
		public static object CreateHierarchyScript(object attatchedTo, Type scriptType)
		{
			object instance; // Initialize object to store.

			instance = Activator.CreateInstance(scriptType); // Create an instance.

			scriptType.GetMethod("SetUp").Invoke(instance, new[] { attatchedTo }); // Invoke SetUp to set the HierarchyObject.

			return instance; // Return stored object.
		}
	}
}
