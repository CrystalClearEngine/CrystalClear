using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using System;

public static class StaticProgramTest
{
	[OnStartEvent]
	public static void OnStart()
	{
		Output.Log("Static program reporting in!");
	}
}