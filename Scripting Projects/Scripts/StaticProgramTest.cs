using CrystalClear.ScriptUtilities;
using CrystalClear.EventSystem.StandardEvents;

public static class StaticProgramTest
{
	[OnStartEvent]
	public static void StaticProgramReport()
	{
		Output.Log("Static program reporting in!");
	}
}