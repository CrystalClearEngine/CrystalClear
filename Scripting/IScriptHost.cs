using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalClear.Scripting
{
	// For anything that can hold scripts!
	// TODO: make HierarchyObject use this.
	interface IScriptHost
	{
	}

	// TODO: I think IHierarchyScript should use this.
	interface IScript<T>
		where T : IScriptHost
	{

	}
}
