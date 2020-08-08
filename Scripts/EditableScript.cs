using static CrystalClear.Input;
using CrystalClear;
using CrystalClear.SerializationSystem;
using CrystalClear.Standard.HierarchyObjects;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.HierarchySystem;
using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.Events;
using CrystalClear.EventSystem.StandardEvents;

[Editable(nameof(Editor), nameof(Creator))]
[IsScript]
public class EditableScript : HierarchyScript<HierarchyObject>
{
	public static void Editor(ref EditorData data)
	{
		data["data"] = AskQuestion("Enter a value for data");
	}

	public static object Creator(EditorData data)
	{
		EditableScript editableScript = new EditableScript();
		
		editableScript.data = data["data"];

		return editableScript;
	}

	private string data;

	[OnStartEvent]
	public void Start()
	{
		Output.Log(data);
	}
}