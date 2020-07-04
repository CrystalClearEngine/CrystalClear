using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.ScriptUtilities;
using CrystalClear;

public static class StaticProgramTest
{
	[OnStartEvent]
	public static void StaticProgramReport()
	{
		Output.Log("Static program reporting in!");
	}
}