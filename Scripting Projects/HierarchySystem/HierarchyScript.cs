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
		public HierarchyScript(T hierarchyObject) //TODO Keep in mind that we might want to force the use of an "approved" type. Then we could use HierarchyObject hierarchyObject as parameter or (HierarchyObject)hierarchyObject in the assignment.
		{
			this.HierarchyObject = hierarchyObject;
		}

		protected HierarchyScript()
		{
		}

		public T HierarchyObject;
	}
}
