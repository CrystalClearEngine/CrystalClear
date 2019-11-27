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
		Output.Log(HierarchyObject.test1.ToString());
		Output.Log(HierarchyObject.test2.ToString());

		HierarchyObject.test1 = 1;
		HierarchyObject.test2 = 2;

		Output.Log(HierarchyObject.test1.ToString());
		Output.Log(HierarchyObject.test2.ToString());
	}
}