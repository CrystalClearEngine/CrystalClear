using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalClear.HierarchySystem;

namespace CrystalClear.ScriptingEngine
{
	/// <summary>
	/// The type that all HierarchyScripts derive from. This like a scripting "interface" for scripts to act through.
	/// </summary>
	/// <typeparam name="T">The type of HierarhcyObjects that this Script will be targeting.</typeparam>
	public abstract class HierarchyScript<T>
	{
		protected HierarchyScript()
		{
		}

		public T HierarchyObject;
	}
}
