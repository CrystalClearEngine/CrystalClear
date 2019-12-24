using CrystalClear.EventSystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.CompilationSystem;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Reflection;
using CrystalClear.HierarchySystem;
using System.Threading;
using static CrystalClear.CrystalClearInformation;

public static class MainClass
{
	private static void Main()
	{
		#region Thread Culture
#if DEBUG
		Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US"); // To ensure google-able exceptions.
#endif
		#endregion

		#region Compilation
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
		#endregion

		#region Script identification and method subscription
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
		#endregion

		#region Storing
		string path = WorkingPath + "storage.bin";

		ScriptStorage scriptStorage = new ScriptStorage(scriptTypes[2], new object[] { "Hello there, I was constructed using this type!" }, scriptObject);

		ScriptStorage.StoreToFile(path, scriptStorage);

		scriptObject.AddScriptManually(ScriptStorage.CreateScriptFromScriptStorageFile(path));
		#endregion

		#region Event raising
		// Raise the start event.
		StartEventClass.Instance.RaiseEvent();
		#endregion

		#region Exit handling
		// Wait for user input before closing the application.
		Console.ReadKey();
		#endregion
	}
}