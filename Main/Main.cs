using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.CompilationSystem;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Reflection;
using CrystalClear.HierarchySystem;
using System.Threading;
using CrystalClear.SerializationSystem;
using static CrystalClear.CrystalClearInformation;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

		#region Script identification
		// Cache the results.
		Type[] typesInCode = compiledAssembly.GetTypes();

		// Find all scripts that are present in the newly compiled assembly.
		Type[] scriptTypes = Script.FindScriptTypesInTypes(typesInCode);
		#endregion

		#region Editor Loop
		// Very basic editor.

		EditorHierarchyObject rootEditorHierarchyObject = new EditorHierarchyObject(typeof(HierarchyRoot), null);
		EditorHierarchyObject currentHierarchyObject = rootEditorHierarchyObject;

		LoopEditor:
		string line = Console.ReadLine();

		// Split the command at space that has not been escaped with a \.
		string[] commandSections = Regex.Split(line, @"(?<!\\)( )");

		foreach (string section in commandSections)
		{
			// Clean up the \'s from the earlier split operation.
			Regex.Replace(section, @"\\ ", string.Empty);
		}
		switch (commandSections[0])
		{
			case "new":
				New(commandSections[1], currentHierarchyObject);
				break;

			case "modify":
				Modify(currentHierarchyObject);
				break;

			case "save":
				Save(commandSections[1]);
				break;

			case "load":
				Load(commandSections[1]);
				break;

			case "select":
				Select(commandSections[1]);
				break;

			case "run":
				goto RunProgram;

			default:
				Console.WriteLine("error");
				break;
		}
		goto LoopEditor;
		RunProgram:
		#endregion

		#region Event raising
		// Raise the start event.
		StartEvent.Instance.RaiseEvent();

		// Create a thread for updating the frame.
		Thread frameUpdateThread = new Thread(FrameUpdateEvent.FrameUpdateLoop);
		// Start the thread.
		frameUpdateThread.Start();

		// Create a thread for updating the physics' time-step.
		Thread physicsUpdateThread = new Thread(() => PhysicsTimeStepEventClass.PhysicsTimeStepLoop());
		// Start the thread.
		physicsUpdateThread.Start();

		// Create a thread for polling input.
		Thread inputPollingThread = new Thread(() => InputPollEvent.InputPollLoop());
		// Start the thread.
		inputPollingThread.Start();
		#endregion

		#region Exit handling
		ExitHandling:
		if (Console.ReadKey().Key == ConsoleKey.Escape)
		{
			// Exit.
			Environment.Exit(1);
		}
		goto ExitHandling;
		#endregion

		void Modify(EditorObject editorObject)
		{
			editorObject.GetModifier();
		}

		void New(string name, EditorHierarchyObject parent)
		{
			parent.LocalHierarchy.Add(name, new EditorHierarchyObject(typeof(ScriptObject), null));
		}

		void Save(string path)
		{
			if (path == string.Empty)
				path = WorkingPath + @"\binary.bin";

			EditorObjectSerialization.SaveToFile(path, rootEditorHierarchyObject);
		}

		void Load(string path)
		{
			if (path == string.Empty)
				path = WorkingPath + @"\binary.bin";

			rootEditorHierarchyObject = (EditorHierarchyObject)EditorObjectSerialization.LoadFromSaveFile(path);
			currentHierarchyObject = rootEditorHierarchyObject;
		}

		void Select(string editorHierarchyName)
		{
			currentHierarchyObject = currentHierarchyObject.LocalHierarchy[editorHierarchyName];
		}
	}
}