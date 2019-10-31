using CrystalClear.EventSystem.Events;
using CrystalClear.ScriptUtils;

[IsScript]
public class HelloWorldExample
{
	[OnStartEvent]
	public void OnStart()
	{
		Output.Log("Hello World");
	}
}