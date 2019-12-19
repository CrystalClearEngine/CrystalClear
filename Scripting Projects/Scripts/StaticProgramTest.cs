using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;

public static class StaticProgramTest
{
	[OnStartEvent]
	public static void StaticProgramReport()
	{
		Output.Log("Static program reporting in!");
	}
}