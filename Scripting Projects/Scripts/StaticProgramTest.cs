using CrystalClear.ScriptingEngine;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using CrystalClear.HierarchySystem;

public static class StaticProgramTest
{
	[OnStartEvent]
	public static void OnStart()
	{
		Output.Log("Static program reporting in!");
	}
}