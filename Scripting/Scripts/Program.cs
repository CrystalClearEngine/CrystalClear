﻿using CrystalClear.Scripting;
using CrystalClear.Scripting.ScriptAttributes;
using System;
using System.Collections.Generic;
using CrystalClear.Scripting.EventSystem;
using CrystalClear.Scripting.EventSystem.Events;

[Script]
public class HelloWorldExample
{
	[OnStartEvent]
	public void OnStart(EventArgs args)
	{
		Console.WriteLine("Hello World");
	}

	public void DynamicallyCallMe()
	{
		Console.WriteLine("Yay I was dynamically called!");
	}

	[OnExitEvent]
	public void OnExit(EventArgs args)
	{
		Console.WriteLine("Bye World");
	}
}