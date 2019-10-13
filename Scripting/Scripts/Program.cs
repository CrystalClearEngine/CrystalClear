using System;
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

	public void DynamicallyCallMe()
	{
		Console.WriteLine("Yay I was dynamically called!");
	}

	public void AndMe()
	{
		Console.WriteLine("Yiey");
	}

	public void IToo()
	{
		Console.WriteLine("Yayy");
	}

	[OnExitEvent]
	public void OnExit(ExitEventArgs args)
	{
		args.Cancel();
		Console.WriteLine("Bye!");
	}
}