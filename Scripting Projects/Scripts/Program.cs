using CrystalClear.EventSystem.Events;
using CrystalClear.ScriptUtils;
using CrystalClear.Standard;
using CrystalClear.Standard.HierarchyObjects;

[IsScript]
public class HelloWorldExample : ScriptObject
{
	[OnStartEvent]
	public void OnStart()
	{
		Output.Log("Hello World");
	}
}