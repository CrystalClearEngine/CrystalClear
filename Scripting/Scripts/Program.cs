﻿using CrystalClear.Scripting;
using CrystalClear.Scripting.ScriptAttributes;
using System;
using System.Collections.Generic;
using CrystalClear.Scripting.EventSystem;
using CrystalClear.Scripting.EventSystem.Events;

[Script]
public class HelloWorldExample
{
	[SubscribeTo(typeof(StartEventHandler))]
	public void OnStart(object o, EventArgs args)
	{
		Console.WriteLine("Hello World");
	}

	[SubscribeTo(typeof(ExitEventHandler))]
	public void OnExit(object o, EventArgs args)
	{
		Console.WriteLine("Bye World");
	}
}