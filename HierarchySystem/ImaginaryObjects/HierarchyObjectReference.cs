using CrystalClear.HierarchySystem;
using static CrystalClear.ScriptUtilities.Utilities.ConsoleInput;

namespace CrystalClear.SerializationSystem
{
	// TODO: finish...
	// TODO: either mirror all properties of the referenced, or just return the referenced.
	[Editable(nameof(Creator), nameof(Editor))]
	public class HierarchyObjectReference : HierarchyObject
	{
		private static HierarchyObject Creator(EditorData data) => HierarchyManager.FollowPath(data["ReferencePath"]);

		private static void Editor(ref EditorData data)
		{
			if (data is null)
			{
				data = new EditorData();
			}

			data["ReferencePath"] = AskQuestion("Enter the path of the HierarchyObject to reference");
		}
	}
}