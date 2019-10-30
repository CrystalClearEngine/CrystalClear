using System;
using CrystalClear.Scripting.EventSystem;
using CrystalClear.Scripting.EventSystem.Events;
using CrystalClear.Scripting.ScriptAttributes;

[Script]
public class HelloWorldExample
{
	[OnStartEvent]
	public void OnStart()
	{
		Console.WriteLine("Hello World");
	}
}