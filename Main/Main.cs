using CrystalClear.EventSystem;
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

		List<EditorHierarchyObject> editorHierarchyObjects = new List<EditorHierarchyObject>();

		LoopEditor:
		string line = Console.ReadLine();
		string[] commands = line.Split(' ');
		switch (commands[0])
		{
			#region Object Management
			case "create":
				Create(commands[1]);
				break;

			case "modify":
				Modify(commands[1]);
				break;

			case "view":
				View();
				break;
			#endregion

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

		void Modify(string v)
		{
			string[] path = v.Split('\\');
			string name = path[path.Length - 1];
			Console.Write($"Modifier for {name}");
		}

		void Create(string v)
		{
			editorHierarchyObjects.Add(new EditorHierarchyObject() { Name = v });
		}
		
		void View()
		{
			foreach (EditorHierarchyObject editorHierarchyObject in editorHierarchyObjects)
			{
				ViewPath(editorHierarchyObject.Name);
			}
		}
		
		void ViewPath(string path)
		{
			Console.WriteLine(editorHierarchyObject.Path);
			ViewPath(editorHierarchyObject.Path);
		}
	}
}