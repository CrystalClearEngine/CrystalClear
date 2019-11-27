using CrystalClear.ScriptingEngine;
using CrystalClear.Standard.Events;
using System;
using System.Reflection;
using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;
using CrystalClear.EventSystem;

public static class MainClass
{
	private static void Main()
	{
		// The files to compile.
		string[] scriptFilesPaths =
		{
			@"E:\dev\crystal clear\Scripting Projects\Scripts\Program.cs"
		};

		// Compile the code.
		Assembly compiledAssembly = Compiler.CompileCode(scriptFilesPaths);

		// If the compiled assembly is null, something went wrong during compilation (there was probably en error in the code).
		if (compiledAssembly == null)
		{
			Console.WriteLine("Compilation failed :( (compiled assembly is null)");
			Console.ReadKey();
			Environment.Exit(-1);
		}

		// Find all scripts that are present in the newly compiled assembly.
		Type[] scriptTypes = Script.FindScriptTypesInAssembly(compiledAssembly);
		ScriptObject scriptObject = new ScriptObject();
		foreach (Type scriptType in scriptTypes)
		{
			scriptObject.AddScript(scriptType);
		}

		// Raise the start event.
		StartEventClass.StartEventInstance.OnEvent();

		// Wait for user input before closing the application.
		Console.ReadKey();
	}
}