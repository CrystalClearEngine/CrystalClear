using CrystalClear.Standard.HierarchyObjects;
using CrystalClear.Standard.Events;
using CrystalClear.ScriptUtilities;
using CrystalClear.EventSystem;

[IsScript]
public class HelloWorldExample : ScriptObject
{
	[SubscribeTo(typeof(StartEventClass))]
	public void OnStart()
	{
		Output.Log("Hello World");
	}
}