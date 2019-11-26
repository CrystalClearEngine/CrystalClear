using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;

[IsScript]
public class HelloWorldExample : ScriptObject
{
	[OnStartEvent]
	public void OnStart()
	{
		Output.Log("Hello World");
		tempValue = 1001;
	}
}