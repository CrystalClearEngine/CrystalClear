using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.HierarchyObjects;
using CrystalClear;

[IsScript]
public class HelloWorldExample : HierarchyScript<ScriptObject>
{
	[OnStartEvent]
	public void PrintHelloWorld()
	{
		Output.Log(HierarchyObject.HelloWorldText);
	}
}