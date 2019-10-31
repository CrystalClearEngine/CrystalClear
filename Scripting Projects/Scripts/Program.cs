using System;
using CrystalClear.EventSystem.Events;
using CrystalClear.ScriptUtils;

[Script]
public class HelloWorldExample
{
	[OnStartEvent]
	public void OnStart()
	{
		Console.WriteLine("Hello World");
	}
}