using CrystalClear.EventSystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.CompilationSystem;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Reflection;

public static class MainClass
{
	private static void Main()
	{
		// The files to compile.
		string[] scriptFilesPaths =
		{
			@"E:\dev\crystal clear\Scripting Projects\Scripts\HelloWorldExample.cs",
			@"E:\dev\crystal clear\Scripting Projects\Scripts\StaticProgramTest.cs",
			@"E:\dev\crystal clear\Scripting Projects\Scripts\StepRoutineTest.cs",
		};

		// Compile our code.
		Assembly compiledAssembly = Compiler.CompileCode(scriptFilesPaths);

		// If the compiled assembly is null then something went wrong during compilation (there was probably en error in the code).
		if (compiledAssembly == null)
		{
			Console.WriteLine("Compilation failed :( (compiled assembly is null)"); // Explain to user.
			Console.ReadKey(); // Wait for user input.
			return; // Exit.
		}

		// Cache the results.
		Type[] typesInCode = compiledAssembly.GetTypes();

		// Find and subscribe event methods in our types.
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
		StartEventClass.Instance.RaiseEvent();

		// Wait for user input before closing the application.
		Console.ReadKey();
	}
}