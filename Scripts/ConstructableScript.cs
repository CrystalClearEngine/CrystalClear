using CrystalClear;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.HierarchyObjects;

[IsScript]
public class ConstructableScript : HierarchyScript<ScriptObject>
{
	public string ToPrint;

	public ConstructableScript()
	{
		ToPrint = "default value";
	}

	public ConstructableScript(string toPrint)
	{
		ToPrint = toPrint;
	}

	[OnStartEvent]
	public void Print()
	{
		Output.Log(ToPrint);
	}
}