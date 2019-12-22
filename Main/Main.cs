using CrystalClear.EventSystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.CompilationSystem;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Reflection;
using CrystalClear.HierarchySystem;
using System.Threading;

public static class MainClass
{
	private static void Main()
	{
#if DEBUG
		Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
#endif

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
		// Add ScriptObject to the LoadedHierarchies list.
		HierarchyManager.AddHierarchy("Experiment ScriptObject", scriptObject);
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

		ScriptStorage scriptStorage = new ScriptStorage(scriptTypes[2], new object[] { "Hello there, I was constructed using this type!" }, scriptObject);

		string path = $@"{Environment.CurrentDirectory}\binary.bin";

		ScriptStorage.StoreToFile(path, scriptStorage);

		scriptObject.AddScriptManually(ScriptStorage.CreateScriptFromScriptStorageFile(path));

		// Raise the start event.
		foreach (Delegate del in StartEventClass.Instance.GetSubscribers())
		{
			Console.WriteLine(del.Method.Name);
		}
		StartEventClass.Instance.RaiseEvent();

		// Wait for user input before closing the application.
		Console.ReadKey();
	}
}