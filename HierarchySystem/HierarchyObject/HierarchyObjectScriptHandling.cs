using System.Collections.Generic;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.HierarchySystem.Scripting.Messages;
using CrystalClear.SerializationSystem.ImaginaryObjects;

namespace CrystalClear.HierarchySystem
{
	public partial class HierarchyObject
	{
		// Script Handling.

		/// <summary>
		///     The Scripts that are currently attached to this object.
		/// </summary>
		public Dictionary<string, Script> AttachedScripts = new Dictionary<string, Script>();

		public void AddScript(string name, ImaginaryScript imaginaryScript)
		{
			AttachedScripts.Add(name, (Script) imaginaryScript.CreateInstance());
		}

		/// <summary>
		///     Adds a Script directly to this HierarchyObject. Note that this will *not* automatically attached the Script to the
		///     HierachyObject.
		/// </summary>
		/// <param name="script">The Script to add.</param>
		public void AddScriptManually(Script script, string name = null)
		{
			if (name is null)
			{
				name = Utilities.EnsureUniqueName(script.ScriptType.Name, AttachedScripts.Keys);
			}

			AttachedScripts.Add(name, script);
		}

		public void RemoveScript(string name)
		{
			new ScriptToBeRemoved().SendTo(AttachedScripts[name]);

			AttachedScripts[name].UnsubscribeAll();

			AttachedScripts.Remove(name);
		}

		protected void RemoveAllScripts()
		{
			foreach (var scriptName in AttachedScripts.Keys)
			{
				RemoveScript(scriptName);
			}
		}
	}
}