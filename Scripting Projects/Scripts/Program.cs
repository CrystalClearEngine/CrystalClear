using CrystalClear.Standard.HierarchyObjects;
using CrystalClear.Standard.Events;
using CrystalClear.ScriptUtilities;

[IsScript]
public class HelloWorldExample : ScriptObject
{
	[OnStartEvent]
	public void OnStart()
	{
		Output.Log("Hello World");
	}
}