using CrystalClear;
using CrystalClear.HierarchySystem;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using static CrystalClear.CrystalClearInformation;
using static CrystalClear.EditorInformation;
using static CrystalClear.Input;

namespace EditorMain
{
	partial class MainClass
	{
		private static void Main()
		{
			#region Project Selection

			Output.Log("Please open or create a new project:");
			ProjectSelection:
			switch (Console.ReadLine())
			{
				case "new":
					ProjectInfo.NewProject(AskQuestion("Pick a path for the new project"),
						AskQuestion("Pick a name for the new project"));
					break;

				case "open":
					try
					{
						ProjectInfo.OpenProject(AskQuestion("Enter the path of the project"));
					}
					catch (ArgumentException)
					{
						goto ProjectSelection;
					}

					break;

				default:
					Output.ErrorLog("command error: unknown command");
					goto ProjectSelection;
			}
			#endregion

			#region Compilation
			// TODO: should update this when a new project is loaded.
			// TODO: make this into a property in ProjectInfo.

			{
				FileInfo[] files = CurrentProject.ScriptsDirectory.GetFiles("*.cs");

				CodeFilePaths = new string[files.Length];

				for (var i = 0; i < files.Length; i++)
					CodeFilePaths[i] = files[i].FullName;
			}

			// Compile the code.
			var compiledAssembly = Compile();

			Analyze(compiledAssembly);

			// TODO: update this when a new ProjectInfo is used.
			var fileSystemWatcher = new FileSystemWatcher(CurrentProject.ScriptsDirectory.FullName, "*.cs");
			fileSystemWatcher.Changed += (_, _1) =>
			{
				Output.Log("Code change detected, recompiling.");
				CodeFilePaths = Directory.GetFiles(CurrentProject.ScriptsDirectory.FullName, "*.cs");
				// TODO: something to wait until the file is ready.
				Thread.Sleep(100); // OTHER THAN THIS LOL
				Compile();
				Analyze(compiledAssembly);
			};
			fileSystemWatcher.EnableRaisingEvents = true;

			#endregion

			#region Editor loop

			// Very basic editor.

			var rootHierarchyObject =
				new ImaginaryHierarchyObject(null, new ImaginaryConstructableObject(typeof(HierarchyRoot)));
			ImaginaryHierarchyObject currentSelectedHierarchyObject = rootHierarchyObject;

			while (true)
			{
				Output.Log();

				var line = Console.ReadLine();

				ParseCommand(line, ref rootHierarchyObject, ref currentSelectedHierarchyObject, compiledAssembly);
			}
			#endregion
		}
	}
}