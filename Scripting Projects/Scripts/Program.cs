using System;
using CrystalClear.EventSystem.Events;
using CrystalClear.ScriptingEngine;

[Script]
public class HelloWorldExample
{
	[OnStartEvent]
	public void OnStart()
	{
		Console.WriteLine("Hello World");
	}
}