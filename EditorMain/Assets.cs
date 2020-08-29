using System;
using System.IO;
using static CrystalClear.EditorInformation;

public static class Assets
{
	const string defaultScript =
		"using CrystalClear;\n" +
		"using CrystalClear.EventSystem.StandardEvents;\n" +
		"using CrystalClear.HierarchySystem.Scripting;\n" +
		"using CrystalClear.ScriptUtilities;\n" +
		"using CrystalClear.Standard.Events;\n" +
		"using CrystalClear.Standard.HierarchyObjects;\n" +
		"\n" +
		"[IsScript]\n" +
		"public class Script\n" +
		"{\n" +
		"	[OnStartEvent]\n" +
		"	public void Start()\n" +
		"	{\n" +
		"	}\n" +
		"	\n" +
		"	[OnFrameUpdateEvent]\n" +
		"	public void FrameUpdate()\n" +
		"	{\n" +
		"	}\n" +
		"}\n";

	public static void CreateNewScript(string scriptName)
	{
		string path = Path.Combine(CurrentProject.ScriptsDirectory.FullName, scriptName + ".cs");

		if (File.Exists(path))
		{
			return;
		}

		using (TextWriter newScript = File.CreateText(path))
		{
			newScript.Write(defaultScript);
			newScript.Flush();
		}
	}

	public static void CreateNewHiearchy(string hierarchyName)
	{
		throw new NotImplementedException();
	}

	public static void DeleteAsset(string assetName)
	{
		throw new NotImplementedException();
	}
}