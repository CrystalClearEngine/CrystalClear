using System;

namespace CrystalClear.HierarchySystem.Scripting.Internal
{
	/// <summary>
	/// The base class for HierarchyScript. Not too much to see here.
	/// </summary>
	public abstract class HierarchyScriptBase
	{

	}
}

namespace CrystalClear.HierarchySystem.Scripting
{
	using CrystalClear.HierarchySystem.Scripting.Internal;
	using System.Collections.Generic;

	/// <summary>
	/// The type that all HierarchyScripts derive from. This is like a scripting "interface" for scripts to act through.
	/// </summary>
	/// <typeparam name="T">The type of HierarhcyObjects that this HierarchyScript will be targeting.</typeparam>
	public abstract class HierarchyScript<T>
		: HierarchyScriptBase
		where T : HierarchyObject
	{
		public void SetUp(T hierarchyObject)
		{
			HierarchyObject = hierarchyObject;
		}

		private WeakReference<T> hierarchyObject = new WeakReference<T>(null);
		public T HierarchyObject
		{
			get
			{
				return hierarchyObject.TryGetTargetExt();
			}

			private set => hierarchyObject.SetTarget(value);
		}

		public Dictionary<string, Script> Scripts => HierarchyObject.AttachedScripts;

		public HierarchyObject Root => HierarchyObject.Root;

		public Hierarchy LocalHierarchy => HierarchyObject.LocalHierarchy;

		public Hierarchy Hierarchy => HierarchyObject.Hierarchy;
	}

	/// <summary>
	/// Static methods for dealing with HierarchyScripts.
	/// </summary>
	public static class HierarchyScript
	{
		/// <summary>
		/// Returns whether or not the specified type is a subclass of HierarchyScript.
		/// </summary>
		/// <param name="toCheck">The type to check.</param>
		/// <returns>whether or not the specified type is a subclass of HierarchyScript.</returns>
		public static bool IsHierarchyScript(this Type toCheck)
		{
			return toCheck.IsSubclassOf(typeof(HierarchyScriptBase));
		}

		public static void SetUp(object scriptInstance, HierarchyObject attachedTo)
		{
			scriptInstance.GetType().GetMethod("SetUp").Invoke(scriptInstance, new[] { attachedTo });
		}
	}
}
