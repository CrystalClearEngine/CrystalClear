using CrystalClear.ScriptingEngine;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using CrystalClear.HierarchySystem;

[IsScript]
public class HelloWorldExample : HierarchyScript<ScriptObject>
{
	[OnStartEvent]
	public void OnStart()
	{
		Output.Log("Hello World");
	}
}