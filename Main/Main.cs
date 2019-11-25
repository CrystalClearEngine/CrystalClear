using CrystalClear.ScriptingEngine;
using CrystalClear.Standard.Events;
using System;
using System.Reflection;
using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;

public static class MainClass
{
	private static void Main()
	{
		string[] scriptFilesPaths =
		{
			@"E:\dev\crystal clear\Scripting Projects\Scripts\Program.cs"
		};

		// Hardcoded code to compile
		Assembly compiledScript = Compiler.CompileCode(scriptFilesPaths);

		//If the compiled assembly is null, something went wrong during compilation (there was probably en error in the code).
		if (compiledScript == null)
		{
			Console.WriteLine("Compilation failed :( (compiled assembly is null)");
			Console.ReadKey();
			Environment.Exit(-1);
		}

		//Find all scripts that are present in the newly compiled assembly
		Type[] scriptTypes = Script.FindScriptTypesInAssembly(compiledScript);
		ScriptObject scriptObject = new ScriptObject();
		foreach (Type scriptType in scriptTypes)
		{
			scriptObject.AddScript(scriptType);
		}

		StartEventClass.StartEventInstance.OnEvent();

		Console.ReadKey();
	}
}