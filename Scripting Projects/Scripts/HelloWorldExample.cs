using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using System;

[IsScript]
public class HelloWorldExample : HierarchyScript<ScriptObject>
{
	[OnStartEvent]
	public void OnStart(object sender, EventArgs args)
	{
		Output.Log(HierarchyObject.HelloWorldText);
	}
}