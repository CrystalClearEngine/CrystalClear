using System;

namespace CrystalClear.HierarchySystem.Scripting
{
	/// <summary>
	/// The type that all HierarchyScripts derive from. This is like a scripting "interface" for scripts to act through.
	/// </summary>
	/// <typeparam name="T">The type of HierarhcyObjects that this HierarchyScript will be targeting.</typeparam>
	public abstract class HierarchyScript<T> where T : HierarchyObject
	{
		public void SetUp(T hierarchyObject)
		{
			HierarchyObject = hierarchyObject;
		}

		public T HierarchyObject;
	}

	/// <summary>
	/// Static methods for dealing with HierarchyScripts.
	/// </summary>
	internal static class HierarchyScript
	{
		/// <summary>
		/// Create an instance of a script deriving from HierarchyScript.
		/// </summary>
		/// <param name="attatchedTo">The HierarchyObject that this script is attatched to.</param>
		/// <param name="scriptType">The type of the script.</param>
		/// <returns>An instance of the script.</returns>
		public static object CreateHierarchyScript(object attatchedTo, Type scriptType)
		{
			// Initialize object to store.
			object instance;

			// Create an instance.
			instance = Activator.CreateInstance(scriptType);

			// Invoke SetUp to set the HierarchyObject.
			scriptType.GetMethod("SetUp").Invoke(instance, new[] { attatchedTo });

			// Return stored object.
			return instance;
		}
	}
}
