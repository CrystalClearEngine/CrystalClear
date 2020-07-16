using CrystalClear;
using CrystalClear.CompilationSystem;
using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.RuntimeMain;
using CrystalClear.SerializationSystem;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading;
using static CrystalClear.EditorInformation;
using static CrystalClear.ScriptUtilities.Utilities.ConsoleInput;

// TODO: make partial. (Methods in one file etc.)
public static class MainClass
{
	// TODO: rename to UserGeneratedCode?
	private static Assembly compiledAssembly => userGeneratedCodeLoadContext.Assemblies.First();

	// TODO: when sourcegenerators are stable, make a [AutoWeakProperty] that makes the property automatically.
	private static WeakReference<AssemblyLoadContext> userGeneratedCodeLoadContextWeakRef = new WeakReference<AssemblyLoadContext>(new AssemblyLoadContext("UserGeneratedCodeLoadContext", isCollectible: true));

	private static AssemblyLoadContext userGeneratedCodeLoadContext
	{
		get => userGeneratedCodeLoadContextWeakRef.TryGetTargetExt();

		set => userGeneratedCodeLoadContextWeakRef.SetTarget(value);
	}

	private static void Main()
	{
		#region Thread Culture
#if DEBUG
		Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US"); // To ensure google-able exceptions.
#endif
		#endregion

		Editor();
	}

	private static void Editor()
	{
		#region Project Selection
		Output.Log("Please open or create a new project:");
		ProjectSelection:
		switch (Console.ReadLine())
		{
			case "new":
				ProjectInfo.NewProject(AskQuestion("Pick a path for the new project"), AskQuestion("Pick a name for the new project"));
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
		// Find all scripts that are present.
		List<Type> scriptTypes = null;

		// Find all HierarchyObject types in the compiled assembly.
		List<Type> hierarchyObjectTypes = null;

		// TODO: should update this when a new project is loaded.
		// TODO: make this into a property in ProjectInfo.
		string[] codeFilePaths;

		using ProgressBar indexingProgressBar = new ProgressBar(3, "Indexing files.");
		{
			{
				FileInfo[] files = CurrentProject.ScriptsDirectory.GetFiles("*.cs");

				indexingProgressBar.Tick("Gathered files.");

				codeFilePaths = new string[files.Length];

				indexingProgressBar.Tick();

				for (int i = 0; i < files.Length; i++)
					codeFilePaths[i] = files[i].FullName;

				indexingProgressBar.Tick("Indexed");
			}
		}

		// Compile our code.
		Compile();

		Analyze();

		// TODO: update this when a new ProjectInfo is used.
		FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(CurrentProject.ScriptsDirectory.FullName, "*.cs");
		fileSystemWatcher.Changed += (object _, FileSystemEventArgs _1) =>
		{
			Output.Log("Code change detected, recompiling.");
			codeFilePaths = Directory.GetFiles(CurrentProject.ScriptsDirectory.FullName, "*.cs");
			Compile();
		};
		fileSystemWatcher.EnableRaisingEvents = true;
		#endregion

		#region Editor loop
		// Very basic editor.

		ImaginaryHierarchyObject rootHierarchyObject = new ImaginaryHierarchyObject(null, new ImaginaryConstructableObject(typeof(HierarchyRoot)));
		ImaginaryHierarchyObject currentSelectedHierarchyObject = rootHierarchyObject;

		LoopEditor:
		Output.Log();

		string line = Console.ReadLine();

		string[] commandSections = line.Split(' ');

		// The actual command recognition.
#if RELEASE
		try
		{
#endif
		switch (commandSections[0])
		{
			case "compile":
				Compile();
				break;

			case "new":
				if (commandSections.Length > 1)
				{
					NewHierarchyObject(commandSections[1]);
				}
				else
				{
					// Use default HierarchyObject name if no name is provided.
					NewHierarchyObject();
				}
				break;

			case "del":
				DeleteHierarchyObject(commandSections.Length < 1 ? GetName(currentSelectedHierarchyObject) : commandSections[1]);
				break;

			case "rename":
				Rename(commandSections[1]);
				break;

			case "modify":
				Modify(commandSections.Length >= 2 ? commandSections[1] : null);
				break;

			case "details":
				Details(commandSections.Length >= 2 ? commandSections[1] : null);
				break;

			case "add":
				AddScript(commandSections.Length >= 2 ? commandSections[1] : null);
				break;

			case "rem":
				RemoveScript(commandSections[1]);
				break;

			case "save":
				Save(commandSections[1]);
				break;

			case "load":
				Load(commandSections[1]);
				break;

			case "pack":
				Pack(commandSections[1]);
				break;

			case "unpack":
				Unpack(commandSections[1]);
				break;

			case "list":
				List(commandSections.Length >= 2 ? commandSections[1] : null);
				break;

			case "select":
				Select(commandSections.Length >= 2 ? commandSections[1] : null);
				break;

			case "export":
				switch (commandSections[1])
				{
					case "prefab":
						ExportPrefab(commandSections[2], commandSections.Length! < 3 ? commandSections[3] : null);
						break;

					case "copy":
						ExportHierarchy(commandSections[2], commandSections.Length! < 3 ? commandSections[3] : null);
						break;

					default:
						Output.ErrorLog("command error: unknown subcommand", true);
						break;
				}
				break;

			case "import":
				switch (commandSections[1])
				{
					case "prefab":
						ImportPrefab(commandSections[2]);
						break;

					case "copy":
						ImportHierarchy(commandSections[2]);
						break;

					default:
						Output.ErrorLog("command error: unknown subcommand");
						break;
				}
				break;

			case "project":
				switch (commandSections[1])
				{
					case "new":
						ProjectInfo.NewProject(AskQuestion("Pick a path for the new project"), AskQuestion("Pick a name for the new project"));
						break;

					case "open":
						ProjectInfo.OpenProject(AskQuestion("Enter the path of the project"));
						break;

					case "modify":
						ProjectInfo.ModifyCurrentProject(AskQuestion($"Pick a new name for {CurrentProject.ProjectName}"), AskYOrNQuestion("Change folder name to match new name?"));
						break;

					default:
						Output.ErrorLog("command error: unknown subcommand");
						break;
				}
				break;

			case "script":
				break;

			case "build":
				Builder.Build(commandSections[1], commandSections[2], new[] { compiledAssembly });
				break;

			case "run":
				goto RunProgram;

			case "exit":
				Environment.Exit(0);
				break;

			default:
				Output.ErrorLog("command error: unknown command");
				break;
		}
#if RELEASE
		}
		#region Error Handling
#pragma warning disable CA1031 // Do not catch general exception types
		catch (ArgumentNullException ex)
		{
			Output.ErrorLog($"command error: incorrect arg ({ex.Message})");
		}
		catch (IndexOutOfRangeException ex)
		{
			Output.ErrorLog($"command error: missing arg ({ex.Message})");
		}
		catch (NotImplementedException ex)
		{
			Output.ErrorLog($"command error: command not implemented ({ex.Message})");
		}
		catch (NotSupportedException ex)
		{
			Output.ErrorLog($"command error: not supported ({ex.Message})");
		}
#pragma warning restore CA1031 // Do not catch general exception types
		#endregion
#endif
		goto LoopEditor;
		#endregion

		#region Running
		RunProgram:

		Console.Write("Choose a name for the hierarchy: ");
		string hierarchyName = Console.ReadLine();

		Output.Log();

		RuntimeMain.Run(new Assembly[] { compiledAssembly }, hierarchyName, rootHierarchyObject);
		#endregion

		#region Exit handling
		ExitHandling:
		if (Console.ReadKey().Key == ConsoleKey.Escape)
		{
			// Exit on escape key.
			RuntimeMain.Stop();
			goto LoopEditor;
		}
		goto ExitHandling;
		#endregion

		#region Editor Methods
		bool Compile()
		{
			Unload();

			using var compilingProgressBar = new ProgressBar(1, "Compiling");

			bool success = Compiler.CompileCode(codeFilePaths, userGeneratedCodeLoadContext);
			compilingProgressBar.Tick("Compiled");

			// If the compiled assembly is null then something went wrong during compilation (there was probably en error in the code).
			if (!success)
			{
				// Explain to user that the compilation failed.
				Output.ErrorLog("compilation error: compilation failed :(", true);
				// TODO: do type identification for Standard regardless.
				return false;
			}

			Output.Log($"Successfuly built {compiledAssembly.GetName()} at location {compiledAssembly.Location}.", ConsoleColor.Black, ConsoleColor.Green);

			CrystalClearInformation.UserAssemblies = new[]
			{
				compiledAssembly,
				Assembly.GetAssembly(typeof(ScriptObject)),
				Assembly.GetAssembly(typeof(HierarchyObject)),
			};

			return true;
		}

		void Analyze()
		{
			using var analysisProgressBar = new ProgressBar(6, "Analyzing");

			#region Type identification
			Assembly standardAssembly = Assembly.GetAssembly(typeof(ScriptObject));
			analysisProgressBar.Tick("Found Standard assembly");

			// Find all scripts that are present in the compiled assembly.
			scriptTypes = Script.FindScriptTypesInAssembly(compiledAssembly).ToList();
			analysisProgressBar.Tick("Found Script types in compiled assembly");
			scriptTypes.AddRange(Script.FindScriptTypesInAssembly(standardAssembly));
			analysisProgressBar.Tick("Found Script types in Standard");

			// Find all HierarchyObject types in the compiled assembly.
			hierarchyObjectTypes = HierarchyObject.FindHierarchyObjectTypesInAssembly(compiledAssembly).ToList();
			analysisProgressBar.Tick("Found HierarchyObject types in compiled assembly");
			// Add the HierarchyObjects defined in standard HierarchyObjects.
			hierarchyObjectTypes.AddRange(HierarchyObject.FindHierarchyObjectTypesInAssembly(standardAssembly));
			analysisProgressBar.Tick("Found HierarchyObject types in Standard");

			analysisProgressBar.Tick("Analyzed");
			#endregion
		}

		void Modify(string toModify = null)
		{
			ImaginaryHierarchyObject hierarchyObjectToModify;
			if (string.IsNullOrEmpty(toModify))
			{
				hierarchyObjectToModify = currentSelectedHierarchyObject;
			}
			else
			{
				if (!currentSelectedHierarchyObject.LocalHierarchy.ContainsKey(toModify))
				{
					Output.ErrorLog($"command error: no HierarchyObject named {toModify} can be found!");
					return;
				}
				hierarchyObjectToModify = currentSelectedHierarchyObject.LocalHierarchy[toModify];
			}

			if (AskYOrNQuestion($"Do you want to change the name of the HierarchyObject? Name = {GetName(hierarchyObjectToModify)}"))
			{
				SetName(hierarchyObjectToModify, AskQuestion("Write the new name"));
			}

			if (hierarchyObjectToModify.ImaginaryObjectBase is ImaginaryEditableObject imaginaryEditableObject)
			{
				EditableSystem.OpenEditor(imaginaryEditableObject.TypeData.GetConstructionType(), ref ((ImaginaryEditableObject)hierarchyObjectToModify.ImaginaryObjectBase).EditorData);
			}
			else if (hierarchyObjectToModify.ImaginaryObjectBase is ImaginaryConstructableObject imaginaryConstructableObject)
			{
				imaginaryConstructableObject.ImaginaryConstructionParameters = GetConstructorParameters(imaginaryConstructableObject.TypeData.GetConstructionType());
			}
			else
			{
				Output.ErrorLog($"{hierarchyObjectToModify.ImaginaryObjectBase.GetType()} cannot be modified using this tool.", true);
			}
		}

		void Details(string toDetail = null)
		{
			ImaginaryHierarchyObject hierarchyObjectToViewDetailsOf;

			if (string.IsNullOrEmpty(toDetail))
			{
				hierarchyObjectToViewDetailsOf = currentSelectedHierarchyObject;
				if (hierarchyObjectToViewDetailsOf.Parent != null)
				{
					toDetail = GetName(hierarchyObjectToViewDetailsOf);
				}
			}
			else
			{
				if (!currentSelectedHierarchyObject.LocalHierarchy.ContainsKey(toDetail))
				{
					Output.ErrorLog($"command error: no HierarchyObject named {toDetail} can be found!");
					return;
				}
				hierarchyObjectToViewDetailsOf = currentSelectedHierarchyObject.LocalHierarchy[toDetail];
			}

			Output.Log($"Details for {hierarchyObjectToViewDetailsOf}:");

			Output.Log($"Name: {toDetail ?? "\b\b is unknown. Presumably the HierarchyObject is root."}");

			Output.Log($"Type: {hierarchyObjectToViewDetailsOf}");

			if (hierarchyObjectToViewDetailsOf.ImaginaryObjectBase is ImaginaryConstructableObject imaginaryConstructable)
			{
				Output.Log("This HierarchyObject uses constructor parameters to be created.");

				Output.Log($"Parameter count: {imaginaryConstructable.ImaginaryConstructionParameters.Length}");

				Console.Write("Parameters: (");
				bool first = true;
				foreach (ImaginaryObject parameter in imaginaryConstructable.ImaginaryConstructionParameters)
				{
					// Put commas after every parameter if unless it's the first parameter.
					if (!first)
					{
						Console.Write(", ");
					}

					Console.Write(parameter.ToString());

					first = false;
				}
				Console.Write(")\n");
			}
			else if (hierarchyObjectToViewDetailsOf.ImaginaryObjectBase is ImaginaryEditableObject imaginaryEditableObject)
			{
				Output.Log("This HierarchyObject uses an Editor to be created and modified.");

				Output.Log($"EditorData count: {imaginaryEditableObject.EditorData.Count}");

				Console.Write("EditorData: (");
				bool first = true;
				foreach (KeyValuePair<string, string> data in imaginaryEditableObject.EditorData)
				{
					// Put commas after every parameter if unless it's the first parameter.
					if (!first)
					{
						Console.Write(", ");
					}

					Console.Write($"{data.Key}: {data.Value}");

					first = false;
				}
				Console.Write(")\n");
			}
			else
			{
				Output.ErrorLog($"Cannot detail the construction of a HierarchyObject of type {hierarchyObjectToViewDetailsOf.ImaginaryObjectBase?.GetType()}.", true);
				// TODO: use reflection to try and detail it anyways?
			}

			// TODO: add LocalHierarchy and AttatchedScripts information here.
		}

		// TODO: Make this support generics (generic HierarchyObjects) will also probably require a change to ImaginaryHierarchyObject. (Script equivalents too so they can support generic Scripts!)
		void NewHierarchyObject(string name = null)
		{
			Type hierarchyObjectType = SelectItem(hierarchyObjectTypes);

			if (string.IsNullOrEmpty(name))
			{
				name = Utilities.EnsureUniqueName(hierarchyObjectType.Name, currentSelectedHierarchyObject.LocalHierarchy.Keys);
			}
			else if (currentSelectedHierarchyObject.LocalHierarchy.ContainsKey(name))
			{
				name = Utilities.EnsureUniqueName(name, currentSelectedHierarchyObject.LocalHierarchy.Keys);
			}

			currentSelectedHierarchyObject.LocalHierarchy.Add(name, CreateImaginaryHierarchyObject(hierarchyObjectType));
			Output.Log($"HierarchyObject {name} has been added!");

			ImaginaryHierarchyObject CreateImaginaryHierarchyObject(Type ofType)
			{
				if (ofType.IsEditable())
				{
					EditorData editorData = EditorData.GetEmpty();
					EditableSystem.OpenEditor(ofType, ref editorData);
					return new ImaginaryHierarchyObject(currentSelectedHierarchyObject, new ImaginaryEditableObject(ofType, editorData));
				}
				else if (ofType.GetConstructors().Length > 0)
				{
					return new ImaginaryHierarchyObject(currentSelectedHierarchyObject, new ImaginaryConstructableObject(ofType, GetConstructorParameters(ofType)));
				}
				else
				{
					return new ImaginaryHierarchyObject(currentSelectedHierarchyObject, new ImaginaryConstructableObject(ofType));
				}
			}
		}

		void DeleteHierarchyObject(string nameOfEditorHierarchyObjectToDelete)
		{
			currentSelectedHierarchyObject.LocalHierarchy.Remove(nameOfEditorHierarchyObjectToDelete);
			Output.Log($"HierarchyObject {nameOfEditorHierarchyObjectToDelete} has been deleted.");
		}

		void Rename(string newName)
		{
			if (currentSelectedHierarchyObject.Parent is null)
			{
				Output.ErrorLog("command error: currently selected HierarchyObject has no parent and has therefore no name and cannot be renamed.");
				return;
			}
			string oldName = GetName(currentSelectedHierarchyObject);
			SetName(currentSelectedHierarchyObject, newName);
			Output.Log($"Renamed {oldName} to {newName}.");
		}

		void AddScript(string name = null)
		{
			Type scriptType = SelectItem(scriptTypes);

			if (name is null)
			{
				name = Utilities.EnsureUniqueName(scriptType.Name, currentSelectedHierarchyObject.AttatchedScripts.Keys);
			}
			else if (currentSelectedHierarchyObject.AttatchedScripts.ContainsKey(name))
			{
				name = Utilities.EnsureUniqueName(name, currentSelectedHierarchyObject.AttatchedScripts.Keys);
			}

			currentSelectedHierarchyObject.AttatchedScripts.Add(name, CreateImaginaryScript(scriptType));

			Output.Log($"Script {name} has been added!");

			static ImaginaryScript CreateImaginaryScript(Type type)
			{
				if (type.IsEditable())
				{
					EditorData editorData = EditorData.GetEmpty();
					EditableSystem.OpenEditor(type, ref editorData);
					return new ImaginaryScript(new ImaginaryEditableObject(type, editorData));
				}
				else if (type.GetConstructors().Length > 0)
				{
					return new ImaginaryScript(new ImaginaryConstructableObject(type, GetConstructorParameters(type)));
				}
				else
				{
					return new ImaginaryScript(new ImaginaryConstructableObject(type));
				}
			}
		}

		void RemoveScript(string name)
		{
			currentSelectedHierarchyObject.AttatchedScripts.Remove(name);
			Output.Log($"Script {name} has been removed.");
		}

		void Save(string path)
		{
			try
			{
				ImaginaryObjectSerialization.SaveToFile(path, rootHierarchyObject);

				Output.Log($"Successfuly saved to location {path}.", ConsoleColor.Black, ConsoleColor.Green);
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (FileNotFoundException)
			{
				Output.ErrorLog("command error: the file cannot be found");
			}
		}

		void Load(string path)
		{
			try
			{
				rootHierarchyObject = ImaginaryObjectSerialization.LoadFromSaveFile<ImaginaryHierarchyObject>(path);
				currentSelectedHierarchyObject = rootHierarchyObject;

				Output.Log($"Successfuly loaded from location {path}.", ConsoleColor.Black, ConsoleColor.Green);

			}
			catch (FileNotFoundException)
			{
				Output.ErrorLog("command error: the file cannot be found");
			}
		}

		void Pack(string path)
		{
			try
			{
				ImaginaryObjectSerialization.PackImaginaryObjectToFile(path, rootHierarchyObject);

				Output.Log($"Successfuly packed to location {path}.", ConsoleColor.Black, ConsoleColor.Green);
			}
			catch (FileNotFoundException)
			{
				Output.ErrorLog("command error: the file cannot be found");
			}
		}

		void Unpack(string path)
		{
			try
			{
				rootHierarchyObject = (ImaginaryHierarchyObject)ImaginaryObjectSerialization.UnpackImaginaryObject(path);
				currentSelectedHierarchyObject = rootHierarchyObject;

				Output.Log($"Successfuly unpacked from location {path}.", ConsoleColor.Black, ConsoleColor.Green);
			}
			catch (FileNotFoundException)
			{
				Output.ErrorLog("command error: the file cannot be found");
			}
		}
#pragma warning restore CA1031 // Do not catch general exception types

		// Lists all HierarchyObjects in the current HierarchyObject's local Hierarchy.
		void List(string toList = null)
		{
			ImaginaryHierarchyObject hierarchyObjectToList;

			if (string.IsNullOrEmpty(toList))
			{
				hierarchyObjectToList = currentSelectedHierarchyObject;
			}
			else
			{
				hierarchyObjectToList = currentSelectedHierarchyObject.LocalHierarchy[toList];
			}

			foreach (string name in currentSelectedHierarchyObject.LocalHierarchy.Keys)
			{
				Output.Log(name);
			}

			Output.Log("A total of " + hierarchyObjectToList.LocalHierarchy.Count + " HierarchyObjects in the local hierarchy.");
		}

		// TODO: add forwardsteps. A selection can be done like this to traverse multiple layers <<< MyFolder > MySubfolder > MyObject
		void Select(string editorObjectSelectQuery = null)
		{
			// Store the status of the currently selected HierarchyObject so we can revert back here.
			ImaginaryHierarchyObject initiallySelected = currentSelectedHierarchyObject;

			// Add selections when the query is empty.
			if (string.IsNullOrEmpty(editorObjectSelectQuery))
			{
				currentSelectedHierarchyObject = SelectItem(currentSelectedHierarchyObject.LocalHierarchy.Values);
				return;
			}

			// Does this query start with a backstep?
			if (editorObjectSelectQuery.StartsWith("<"))
			{
				// Get the count of backsteps.
				int backStepCount = editorObjectSelectQuery.TakeWhile((char c) => (c == '<')).Count();

				// Backstep.
				for (int i = 0; i < backStepCount; i++)
				{
					// Check if the HierarchyObject actually has a parent.
					if (currentSelectedHierarchyObject.Parent is null)
					{
						Output.ErrorLog($"error: {currentSelectedHierarchyObject} does not have a parent. Reverting the select.");
						currentSelectedHierarchyObject = initiallySelected;
						return;
					}
					// Perform the backstep by going back to the parent of currentEditorHierarchyObject.
					currentSelectedHierarchyObject = currentSelectedHierarchyObject.Parent;
				}

				// Was the whole query a backstep?
				if (editorObjectSelectQuery.EndsWith("<"))
				{
					// Then we don't need to do anything else.
					return;
				}

				// Remove the backstep characters from the query, since they have already been counted.
				editorObjectSelectQuery = editorObjectSelectQuery.Remove(0, backStepCount);
			}
			// Does the query contain backsteps in another location than the start of the string? That's illegal.
			else if (editorObjectSelectQuery.Contains('<'))
			{
				Output.ErrorLog("error: backsteps ('<') cannot be located anywhere else in the query other than at the start.");
			}

			if (!currentSelectedHierarchyObject.LocalHierarchy.ContainsKey(editorObjectSelectQuery))
			{
				Output.ErrorLog($"error: the requested HierarchyObject doesn't exist. Name = {editorObjectSelectQuery}");
				currentSelectedHierarchyObject = initiallySelected;
				return;
			}

			currentSelectedHierarchyObject = currentSelectedHierarchyObject.LocalHierarchy[editorObjectSelectQuery];
		}

		void ImportPrefab(string prefabPath)
		{
			HierarchyPrefab imaginaryHierarchyPrefab = ImaginaryObjectSerialization.LoadFromSaveFile<HierarchyPrefab>(prefabPath);

			imaginaryHierarchyPrefab.PrefabPath = prefabPath;

			imaginaryHierarchyPrefab.Parent = currentSelectedHierarchyObject;

			currentSelectedHierarchyObject.LocalHierarchy.Add(
				imaginaryHierarchyPrefab.PrefabName,
				imaginaryHierarchyPrefab.GetNonPrefab());
		}

		void ExportPrefab(string exportPath, string name = null)
		{
			HierarchyPrefab imaginaryHierarchyPrefab = new HierarchyPrefab(currentSelectedHierarchyObject, name is null ? GetName(currentSelectedHierarchyObject) : name, AskQuestion("What should the path of this Hierarchy prefab be?"));
			ImaginaryObjectSerialization.SaveToFile(exportPath, imaginaryHierarchyPrefab);
		}

		void ImportHierarchy(string hierarchyPath)
		{
			ImaginaryHierarchy imaginaryHierarchy = ImaginaryObjectSerialization.LoadFromSaveFile<ImaginaryHierarchy>(hierarchyPath);

			imaginaryHierarchy.Parent = currentSelectedHierarchyObject;

			currentSelectedHierarchyObject.LocalHierarchy.Add(
				imaginaryHierarchy.HierarchyName,
				imaginaryHierarchy.GetHierarchyObject());
		}

		void ExportHierarchy(string exportPath, string name = null)
		{
			ImaginaryHierarchy imaginaryHierarchy = new ImaginaryHierarchy(currentSelectedHierarchyObject, name is null ? GetName(currentSelectedHierarchyObject) : name);
			ImaginaryObjectSerialization.SaveToFile(exportPath, imaginaryHierarchy);
		}

		string GetName(ImaginaryHierarchyObject toName)
		{
			if (toName.Parent is null)
			{
				return string.Empty;
			}
			return toName.Parent.LocalHierarchy.First(x => ReferenceEquals(x.Value, toName)).Key;
		}

		void SetName(ImaginaryHierarchyObject toName, string newName)
		{
			newName = Utilities.EnsureUniqueName(newName, toName.LocalHierarchy.Keys);
			toName.Parent.LocalHierarchy.Remove(toName.Parent.LocalHierarchy.First(x => ReferenceEquals(x.Value, toName)).Key);
			toName.Parent.LocalHierarchy.Add(newName, toName);
		}

		static T SelectItem<T>(IEnumerable<T> collection)
		{
			selection:
			// Get number of items in the provided collection.
			int count = collection.Count();

			if (count == 0)
			{
				throw new ArgumentException("Selection collection was empty.");
			}
			else if (count == 1)
			{
				Output.Log($"Defaulted to {collection.First()}.");
				return collection.First();
			}

			Output.Log($"Select an item of type {typeof(T).FullName} from this list:");
			int i = 0; // Either this should start at one and the .../{count - 1}... part should not have - 1 or we keep it as is.
			foreach (T item in collection)
			{
				Output.Log($"Item ({i}/{count - 1}): {item.ToString()}");
				getInput:
				Console.Write("Select? Y/N: ");
				char readChar = Console.ReadKey().KeyChar;
				Output.Log();
				if (readChar == 'Y' || readChar == 'y')
				{
					Output.Log($"Selected {item.ToString()}");
					return item;
				}
				else if (readChar == 'N' || readChar == 'n')
				{
					i++;
					continue;
				}
				else
				{
					Output.ErrorLog("Invalid input.", true);
					goto getInput;
				}
			}
			Output.Log("An item needs to be selected!");
			goto selection;
		}

		static ImaginaryObject[] GetConstructorParameters(Type type)
		{
			Output.Log($"Constructor parameter wizard for {type.FullName}.");

			Output.Log("Please select a constructor.");
			ConstructorInfo constructorInfo = SelectItem(type.GetConstructors());
			ParameterInfo[] parameterInfoArray = constructorInfo.GetParameters();

			ImaginaryObject[] parameters = new ImaginaryObject[parameterInfoArray.Length];

			Output.Log("Now provide values for the different parameters.");
			for (int i = 0; i < parameterInfoArray.Length; i++)
			{
				ParameterInfo parameter = parameterInfoArray[i];

				if (parameter.IsOptional)
				{
					if (AskYOrNQuestion($"{parameter.Name} is optional, and defaults to {(parameter.DefaultValue is null ? "null" : parameter.DefaultValue)}, do you want to change it?"))
					{
						parameters[i] = null;
					}
				}
				else
				{
					Output.Log($"{parameter.Name}:");
					parameters[i] = CreateImaginaryObject(parameter.ParameterType);
				}
			}

			return parameters;
		}

		static ImaginaryObject CreateImaginaryObject(Type type)
		{
			if (type.IsEditable())
			{
				EditorData editorData = EditorData.GetEmpty();
				EditableSystem.OpenEditor(type, ref editorData);
				return new ImaginaryEditableObject(type, editorData);
			}
			else if (type.IsEnum)
			{
				return new ImaginaryEnum(type, SelectItem(Enum.GetNames(type)));
			}
			else if (type.QualifiesAsPrimitive())
			{
				return new ImaginaryPrimitive(Convert.ChangeType(Console.ReadLine(), type));
			}
			else if (type.GetConstructors().Length > 0)
			{
				return new ImaginaryConstructableObject(type, GetConstructorParameters(type));
			}
			else
			{
				return new ImaginaryConstructableObject(type);
			}
		}
		#endregion
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Unload()
	{
		using var unloadingProgressBar = new ProgressBar(2, "Unloading");

		CrystalClearInformation.UserAssemblies = null;

		userGeneratedCodeLoadContext.Unloading += (_) => unloadingProgressBar.Tick();

		userGeneratedCodeLoadContext.Unload();
		unloadingProgressBar.Tick();

		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();

		userGeneratedCodeLoadContextWeakRef = new WeakReference<AssemblyLoadContext>(new AssemblyLoadContext("UserGeneratedCodeLoadContext", isCollectible: true));
	}
}