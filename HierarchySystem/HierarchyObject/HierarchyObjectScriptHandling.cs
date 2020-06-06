using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObjectProperties
	{
		// Script Handling.
		/// <summary>
		/// Adds a Script to the HierarchyObject using the type as name.
		/// </summary>
		/// <param name="hierarchyObject">The HierarchyObject to add the Script to.</param>
		/// <param name="script">The Script to add to the HierarchyObject.</param>
		/// <returns>The resulting HierarchyObject.</returns>

		public static HierarchyObjectProperties operator +(HierarchyObjectProperties hierarchyObject, Script script)
		{
			HierarchyObjectProperties result = hierarchyObject;
			result.AddScriptManually(script);
			return result;
		}

		public static HierarchyObjectProperties operator +(HierarchyObjectProperties hierarchyObject, Type scriptType)
		{
			HierarchyObjectProperties result = hierarchyObject;
			result.AddScript(scriptType);
			return result;
		}

		/// <summary>
		/// The Scripts that are currently attatched to this object.
		/// </summary>
		public Dictionary<string, Script> AttatchedScripts = new Dictionary<string, Script>();

		/// <summary>
		/// Adds a HiearchyScript based on the specified type to this HierarchyObject.
		/// </summary>
		/// <param name="scriptType">The type of the Script to add.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		public void AddHierarchyScript(Type scriptType, object[] constructorParameters = null)
		{
			AddScriptManually(new Script(this, scriptType, constructorParameters));
		}

		/// <summary>
		/// Adds a Script of any type other than HierarchyScript to this HiearchyObject.
		/// </summary>
		/// <param name="scriptType">The type of the Script to add.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		public void AddNonHiearchyScriptScript(Type scriptType, object[] constructorParameters = null)
		{
			AddScriptManually(new Script(scriptType, constructorParameters));
		}

		/// <summary>
		/// Adds a Script of any type to this HiearchyObject. If this is a HierarchyScript or not will be automatically detected.
		/// </summary>
		/// <param name="scriptType">The type of the Script to add.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		public void AddScript(Type scriptType, object[] constructorParameters = null)
		{
			AddScriptManually(new Script(scriptType, constructorParameters, this));
		}

		/// <summary>
		/// Adds a Script directly to this HierarchyObject. Note that this will *not* automatically attatch the Script to the HierachyObject.
		/// </summary>
		/// <param name="script">The Script to add.</param>
		public void AddScriptManually(Script script, string name = null)
		{
			if (name is null)
			{
				name = Utilities.EnsureUniqueName(script.ScriptType.Name, AttatchedScripts.Keys);
			}

			AttatchedScripts.Add(name, script);
		}

		public void RemoveScript(string name)
		{
			AttatchedScripts[name].UnsubscribeAll();
			AttatchedScripts.Remove(name);
		}

		protected void RemoveAllScripts()
		{
			foreach (KeyValuePair<string, Script> scriptKeyValuePair in AttatchedScripts)
			{
				scriptKeyValuePair.Value.UnsubscribeAll();
				AttatchedScripts.Remove(scriptKeyValuePair.Key);
			}
		}
	}
}
