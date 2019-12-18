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
			@"E:\dev\crystal clear\Scripting Projects\Scripts\ConstructableScript.cs",
		};

		// Compile our code.
		Assembly compiledAssembly = Compiler.CompileCode(scriptFilesPaths);

		// If the compiled assembly is null then something went wrong during compilation (there was probably en error in the code).
		if (compiledAssembly == null)
		{
			// Explain to user that the compilation failed.
			Console.WriteLine("Compilation failed :( (compiled assembly is null)");
			// Wait for user input.
			Console.ReadKey();
			// Return to exit.
			return;
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

		// Show some temporary debug info about the compiled scripts.
		//foreach (Type scriptType in scriptTypes)
		//{
		//	Console.WriteLine($"{{ {scriptType.Name} contains these constructors:");
		//	foreach (ConstructorInfo constructor in scriptType.GetConstructors())
		//	{
		//		Console.Write($"    Constructor with {constructor.GetParameters().Length} parameters:");
		//		foreach (ParameterInfo parameter in constructor.GetParameters())
		//		{
		//			Console.Write(" " + parameter.Name + "{");
		//			Console.Write($"Optional: {parameter.IsOptional}, ");
		//			Console.Write($"In: {parameter.IsIn}, ");
		//			Console.Write($"Out: {parameter.IsOut}, ");
		//			Console.Write($"Type: {parameter.ParameterType}");
		//			Console.Write("},");
		//		}
		//		Console.WriteLine("  ;");
		//	}
		//	Console.WriteLine("}");
		//}

		ScriptStorage scriptStorage = new ScriptStorage(scriptTypes[2], attatchedTo: scriptObject);
		scriptObject.AttatchedScripts.RemoveAt(2);
		scriptObject.AddScriptManually(scriptStorage.CreateScript());

		// Raise the start event.
		StartEventClass.Instance.RaiseEvent();

		// Wait for user input before closing the application.
		Console.ReadKey();

		foreach (Delegate deleg in StartEventClass.Instance.GetSubscribers())
		{
			Console.WriteLine(deleg.Method.Name);
		}

		StartEventClass.Instance.RaiseEvent();
		Console.ReadKey();
	}
}