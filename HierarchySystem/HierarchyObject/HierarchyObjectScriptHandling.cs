using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using System;
using System.Collections.Generic;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObject
	{
		// Script Handling.

		/// <summary>
		/// The Scripts that are currently attatched to this object.
		/// </summary>
		public Dictionary<string, Script> AttatchedScripts = new Dictionary<string, Script>();

		public void AddScript(string name, ImaginaryScript imaginaryScript)
		{
			AttatchedScripts.Add(name, (Script)imaginaryScript.CreateInstance());
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
