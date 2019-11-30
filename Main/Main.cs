using CrystalClear.ScriptingEngine;
using CrystalClear.Standard.Events;
using System;
using System.Reflection;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.Standard.HierarchyObjects;
using CrystalClear.EventSystem;

public static class MainClass
{
	private static void Main()
	{
		// The files to compile.
		string[] scriptFilesPaths =
		{
			@"E:\dev\crystal clear\Scripting Projects\Scripts\HelloWorldExample.cs",
			@"E:\dev\crystal clear\Scripting Projects\Scripts\StaticProgramTest.cs"
		};

		// Compile our code.
		Assembly compiledAssembly = Compiler.CompileCode(scriptFilesPaths);

		// If the compiled assembly is null something went wrong during compilation (there was probably en error in the code).
		if (compiledAssembly == null)
		{
			Console.WriteLine("Compilation failed :( (compiled assembly is null)"); // Explain to user.
			Console.ReadKey(); // Wait for user input.
			return; // Exit.
		}

		// Cache the results.
		Type[] typesInCode = compiledAssembly.GetTypes();

		EventSystem.FindAndSubscribeEventMethods(typesInCode);

		// Find all scripts that are present in the newly compiled assembly.
		Type[] scriptTypes = Script.FindScriptTypesInTypes(typesInCode);
		// Create a ScriptObject to experiment on. Muahaha!
		ScriptObject scriptObject = new ScriptObject();
		// Add the scripts to scriptObject.
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