using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalClear.HierarchySystem;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// The type that all HierarchyScripts derive from. This is like a scripting "interface" for scripts to act through.
	/// </summary>
	/// <typeparam name="T">The type of HierarhcyObjects that this HierarchyScript will be targeting.</typeparam>
	public abstract class HierarchyScript<T>
	{
		public T HierarchyObject;

		public static HierarchyScript<Type> CreateHierarchyScript<Type>(Type attatchedTo, System.Type scriptType)
		{
			HierarchyScript<Type> hierarchyScript;

			hierarchyScript = (HierarchyScript<Type>)Activator.CreateInstance(scriptType);

			hierarchyScript.HierarchyObject = attatchedTo;

			return hierarchyScript;
		}
	}

	internal static class HierarchyScript
	{
		public static object CreateHierarchyScript(HierarchyObject attatchedTo, Type scriptType)
		{
			object instance; // Initialize object to store.

			instance = scriptType // Set instance.
				.BaseType // The type which scriptType directly inherits from.
				.GetMethod("CreateHierarchyScript") // Get the method called CreateHierarchyScript.
				.MakeGenericMethod( // It is a generic method so treat it as such.
				attatchedTo.GetType()) // Use the type from attatchedTo.
				.Invoke(null, new object[] { attatchedTo, scriptType }); // Invoke the CreateHierarchyScript method and use the return.

			return instance; // Return stored object
		}
	}
}
