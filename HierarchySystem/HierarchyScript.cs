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
				T _ = null;
				hierarchyObject.TryGetTarget(out _);
				return _;
			}

			private set => hierarchyObject.SetTarget(value);
		}

		public Dictionary<string, Script> Scripts => HierarchyObject.AttatchedScripts;

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
		/// Returns wether or not the specified type is a subclass of HierarchyScript.
		/// </summary>
		/// <param name="toCheck">The type to check.</param>
		/// <returns>Wether or not the specified type is a subclass of HierarchyScript.</returns>
		public static bool IsHierarchyScript(Type toCheck)
		{
			return toCheck.IsSubclassOf(typeof(HierarchyScriptBase));
		}

		/// <summary>
		/// Create an instance of a script deriving from HierarchyScript.
		/// </summary>
		/// <param name="attatchedTo">The HierarchyObject that this script is attatched to.</param>
		/// <param name="scriptType">The type of the script.</param>
		/// <returns>An instance of the script.</returns>
		public static object CreateHierarchyScript(object attatchedTo, Type scriptType, object[] constructorParameters = null)
		{
			// Initialize object to store.
			object instance;

			// Is this type static?
			if (scriptType.IsAbstract && scriptType.IsSealed)
			{
				throw new Exception($"A HierarchyScript cannot be static. What happened here? Type = {scriptType.FullName}");
			}

			// Are any constructor parameters provided?
			if (constructorParameters != null)
			{
				// Create an instance of the type using the provided paramters.
				instance = Activator.CreateInstance(scriptType, constructorParameters);
			}
			// Create an instance without parameters.
			else
			{
				// Create an instance of the type.
				instance = Activator.CreateInstance(scriptType);
			}

			// TODO: add error handling for types incompatible with this specific HierarchyScript.
			// Invoke SetUp to set the HierarchyObject up.
			scriptType.GetMethod("SetUp").Invoke(instance, new[] { attatchedTo });

			// Return stored object.
			return instance;
		}
	}
}
