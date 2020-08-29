using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using CrystalClear;
using CrystalClear.CompilationSystem;
using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.SerializationSystem;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using static CrystalClear.EditorInformation;
using static CrystalClear.Input;

partial class MainClass
{
	private static void Editor()
	{
		#region Compilation

		// Find all scripts that are present.
		List<Type> scriptTypes = null;

		// Find all HierarchyObject types in the compiled assembly.
		List<Type> hierarchyObjectTypes = null;

		// TODO: should update this when a new project is loaded.
		// TODO: make this into a property in ProjectInfo.
		string[] codeFilePaths;

		{
			FileInfo[] files = CurrentProject.ScriptsDirectory.GetFiles("*.cs");

			codeFilePaths = new string[files.Length];

			for (var i = 0; i < files.Length; i++)
				codeFilePaths[i] = files[i].FullName;
		}

		// Compile our code.
		Compile();

		Analyze();

		// TODO: update this when a new ProjectInfo is used.
		var fileSystemWatcher = new FileSystemWatcher(CurrentProject.ScriptsDirectory.FullName, "*.cs");
		fileSystemWatcher.Changed += (_, _1) =>
		{
			Output.Log("Code change detected, recompiling.");
			codeFilePaths = Directory.GetFiles(CurrentProject.ScriptsDirectory.FullName, "*.cs");
			// TODO: something to wait until the file is ready.
			Thread.Sleep(100); // OTHER THAN THIS LOL
			Compile();
			Analyze();
		};
		fileSystemWatcher.EnableRaisingEvents = true;

		#endregion

		#region Editor loop

		// Very basic editor.

		var rootHierarchyObject =
			new ImaginaryHierarchyObject(null, new ImaginaryConstructableObject(typeof(HierarchyRoot)));
		ImaginaryHierarchyObject currentSelectedHierarchyObject = rootHierarchyObject;

		LoopEditor:
		Output.Log();

		var line = Console.ReadLine();

		string[] commandSections = line.Split(' ');

		// The actual command recognition.
#if RELEASE
		try
		{
#endif
		switch (commandSections[0])
		{
			case "asset":
				switch (commandSections[1])
				{
					case "new":
						switch (commandSections[2])
						{
							case "script":
								AssetManagement.CreateNewScript(commandSections[3]);
								break;

							case "hierarchy":
								AssetManagement.CreateNewHiearchy(commandSections[3]);
								break;
						}
						break;

					case "delete":
						AssetManagement.DeleteAsset(commandSections[3]);
						break;

					case "list":
						break;
				}
				break;

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
				DeleteHierarchyObject(commandSections.Length < 1
					? GetName(currentSelectedHierarchyObject)
					: commandSections[1]);
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
						Output.ErrorLog("command error: unknown subcommand");
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
				return;

			case "script":
				break;

			case "build":
				Builder.Build(commandSections[1], commandSections[2], new[] {compiledAssembly});
				break;

			case "run":
				Run(rootHierarchyObject);
				break;

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

		#region Editor Methods

		bool Compile()
		{
			compiledAssembly = Compiler.CompileCode(codeFilePaths);

			// If the compiled assembly is null then something went wrong during compilation (there was probably en error in the code).
			if (compiledAssembly is null)
			{
				// Explain to user that the compilation failed.
				Output.ErrorLog("compilation error: compilation failed :(");
				// TODO: do type identification for Standard regardless.
				return false;
			}

			Output.Log($"Successfuly built {compiledAssembly.GetName()} at location {compiledAssembly.Location}.",
				ConsoleColor.Black, ConsoleColor.Green);

			return true;
		}

		void Analyze()
		{
			if (compiledAssembly is null)
				return;

			#region Type identification

			var standardAssembly = Assembly.GetAssembly(typeof(ScriptObject));

			// Find all scripts that are present in the compiled assembly.
			scriptTypes = Script.FindScriptTypesInAssembly(compiledAssembly).ToList();
			scriptTypes.AddRange(Script.FindScriptTypesInAssembly(standardAssembly));

			// Find all HierarchyObject types in the compiled assembly.
			hierarchyObjectTypes = HierarchyObject.FindHierarchyObjectTypesInAssembly(compiledAssembly).ToList();

			// Add the HierarchyObjects defined in standard HierarchyObjects.
			hierarchyObjectTypes.AddRange(HierarchyObject.FindHierarchyObjectTypesInAssembly(standardAssembly));

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

			if (AskYOrNQuestion(
				$"Do you want to change the name of the HierarchyObject? Name = {GetName(hierarchyObjectToModify)}"))
			{
				SetName(hierarchyObjectToModify, AskQuestion("Write the new name"));
			}

			if (hierarchyObjectToModify.ImaginaryObjectBase is ImaginaryEditableObject imaginaryEditableObject)
			{
				EditableSystem.OpenEditor(imaginaryEditableObject.TypeData.GetConstructionType(),
					ref ((ImaginaryEditableObject) hierarchyObjectToModify.ImaginaryObjectBase).EditorData);
			}
			else if (hierarchyObjectToModify.ImaginaryObjectBase is ImaginaryConstructableObject
				imaginaryConstructableObject)
			{
				imaginaryConstructableObject.ImaginaryConstructionParameters =
					GetConstructorParameters(imaginaryConstructableObject.TypeData.GetConstructionType());
			}
			else
			{
				Output.ErrorLog(
					$"{hierarchyObjectToModify.ImaginaryObjectBase.GetType()} cannot be modified using this tool.");
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

			if (hierarchyObjectToViewDetailsOf.ImaginaryObjectBase is ImaginaryConstructableObject
				imaginaryConstructable)
			{
				Output.Log("This HierarchyObject uses constructor parameters to be created.");

				Output.Log($"Parameter count: {imaginaryConstructable.ImaginaryConstructionParameters.Length}");

				Console.Write("Parameters: (");
				var first = true;
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
			else if (hierarchyObjectToViewDetailsOf.ImaginaryObjectBase is ImaginaryEditableObject
				imaginaryEditableObject)
			{
				Output.Log("This HierarchyObject uses an Editor to be created and modified.");

				Output.Log($"EditorData count: {imaginaryEditableObject.EditorData.Count}");

				Console.Write("EditorData: (");
				var first = true;
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
				Output.ErrorLog(
					$"Cannot detail the construction of a HierarchyObject of type {hierarchyObjectToViewDetailsOf.ImaginaryObjectBase?.GetType()}.");
				// TODO: use reflection to try and detail it anyways?
			}

			// TODO: add LocalHierarchy and AttachedScripts information here.
		}

		// TODO: Make this support generics (generic HierarchyObjects) will also probably require a change to ImaginaryHierarchyObject. (Script equivalents too so they can support generic Scripts!)
		void NewHierarchyObject(string name = null)
		{
			Type hierarchyObjectType = SelectItem(hierarchyObjectTypes);

			if (string.IsNullOrEmpty(name))
			{
				name = Utilities.EnsureUniqueName(hierarchyObjectType.Name,
					currentSelectedHierarchyObject.LocalHierarchy.Keys);
			}
			else if (currentSelectedHierarchyObject.LocalHierarchy.ContainsKey(name))
			{
				name = Utilities.EnsureUniqueName(name, currentSelectedHierarchyObject.LocalHierarchy.Keys);
			}

			currentSelectedHierarchyObject.LocalHierarchy.Add(name,
				CreateImaginaryHierarchyObject(hierarchyObjectType));
			Output.Log($"HierarchyObject {name} has been added!");

			ImaginaryHierarchyObject CreateImaginaryHierarchyObject(Type ofType)
			{
				if (ofType.IsEditable())
				{
					EditorData editorData = EditorData.GetEmpty();
					EditableSystem.OpenEditor(ofType, ref editorData);
					return new ImaginaryHierarchyObject(currentSelectedHierarchyObject,
						new ImaginaryEditableObject(ofType, editorData));
				}

				if (ofType.GetConstructors().Length > 0)
				{
					return new ImaginaryHierarchyObject(currentSelectedHierarchyObject,
						new ImaginaryConstructableObject(ofType, GetConstructorParameters(ofType)));
				}

				return new ImaginaryHierarchyObject(currentSelectedHierarchyObject,
					new ImaginaryConstructableObject(ofType));
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
				Output.ErrorLog(
					"command error: currently selected HierarchyObject has no parent and has therefore no name and cannot be renamed.");
				return;
			}

			var oldName = GetName(currentSelectedHierarchyObject);
			SetName(currentSelectedHierarchyObject, newName);
			Output.Log($"Renamed {oldName} to {newName}.");
		}

		void AddScript(string name = null)
		{
			Type scriptType = SelectItem(scriptTypes);

			if (name is null)
			{
				name = Utilities.EnsureUniqueName(scriptType.Name, currentSelectedHierarchyObject.AttachedScripts.Keys);
			}
			else if (currentSelectedHierarchyObject.AttachedScripts.ContainsKey(name))
			{
				name = Utilities.EnsureUniqueName(name, currentSelectedHierarchyObject.AttachedScripts.Keys);
			}

			currentSelectedHierarchyObject.AttachedScripts.Add(name, CreateImaginaryScript(scriptType));

			Output.Log($"Script {name} has been added!");

			static ImaginaryScript CreateImaginaryScript(Type type)
			{
				if (type.IsEditable())
				{
					EditorData editorData = EditorData.GetEmpty();
					EditableSystem.OpenEditor(type, ref editorData);
					return new ImaginaryScript(new ImaginaryEditableObject(type, editorData));
				}

				if (type.GetConstructors().Length > 0)
				{
					return new ImaginaryScript(new ImaginaryConstructableObject(type, GetConstructorParameters(type)));
				}

				return new ImaginaryScript(new ImaginaryConstructableObject(type));
			}
		}

		void RemoveScript(string name)
		{
			currentSelectedHierarchyObject.AttachedScripts.Remove(name);
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
				rootHierarchyObject =
					(ImaginaryHierarchyObject) ImaginaryObjectSerialization.UnpackImaginaryObject(path);
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

			foreach (var name in currentSelectedHierarchyObject.LocalHierarchy.Keys)
			{
				Output.Log(name);
			}

			Output.Log("A total of " + hierarchyObjectToList.LocalHierarchy.Count +
			           " HierarchyObjects in the local hierarchy.");
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
				var backStepCount = editorObjectSelectQuery.TakeWhile(c => c == '<').Count();

				// Backstep.
				for (var i = 0; i < backStepCount; i++)
				{
					// Check if the HierarchyObject actually has a parent.
					if (currentSelectedHierarchyObject.Parent is null)
					{
						Output.ErrorLog(
							$"error: {currentSelectedHierarchyObject} does not have a parent. Reverting the select.");
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
				Output.ErrorLog(
					"error: backsteps ('<') cannot be located anywhere else in the query other than at the start.");
			}

			if (!currentSelectedHierarchyObject.LocalHierarchy.ContainsKey(editorObjectSelectQuery))
			{
				Output.ErrorLog(
					$"error: the requested HierarchyObject doesn't exist. Name = {editorObjectSelectQuery}");
				currentSelectedHierarchyObject = initiallySelected;
				return;
			}

			currentSelectedHierarchyObject = currentSelectedHierarchyObject.LocalHierarchy[editorObjectSelectQuery];
		}

		void ImportPrefab(string prefabPath)
		{
			var imaginaryHierarchyPrefab = ImaginaryObjectSerialization.LoadFromSaveFile<HierarchyPrefab>(prefabPath);

			imaginaryHierarchyPrefab.PrefabPath = prefabPath;

			imaginaryHierarchyPrefab.Parent = currentSelectedHierarchyObject;

			currentSelectedHierarchyObject.LocalHierarchy.Add(
				imaginaryHierarchyPrefab.PrefabName,
				imaginaryHierarchyPrefab.GetNonPrefab());
		}

		void ExportPrefab(string exportPath, string name = null)
		{
			var imaginaryHierarchyPrefab = new HierarchyPrefab(currentSelectedHierarchyObject,
				name is null ? GetName(currentSelectedHierarchyObject) : name,
				AskQuestion("What should the path of this Hierarchy prefab be?"));
			ImaginaryObjectSerialization.SaveToFile(exportPath, imaginaryHierarchyPrefab);
		}

		void ImportHierarchy(string hierarchyPath)
		{
			var imaginaryHierarchy = ImaginaryObjectSerialization.LoadFromSaveFile<ImaginaryHierarchy>(hierarchyPath);

			imaginaryHierarchy.Parent = currentSelectedHierarchyObject;

			currentSelectedHierarchyObject.LocalHierarchy.Add(
				imaginaryHierarchy.HierarchyName,
				imaginaryHierarchy.GetHierarchyObject());
		}

		void ExportHierarchy(string exportPath, string name = null)
		{
			var imaginaryHierarchy = new ImaginaryHierarchy(currentSelectedHierarchyObject,
				name is null ? GetName(currentSelectedHierarchyObject) : name);
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
			toName.Parent.LocalHierarchy.Remove(toName.Parent.LocalHierarchy
				.First(x => ReferenceEquals(x.Value, toName)).Key);
			toName.Parent.LocalHierarchy.Add(newName, toName);
		}

		static T SelectItem<T>(IEnumerable<T> collection)
		{
			selection:
			// Get number of items in the provided collection.
			var count = collection.Count();

			if (count == 0)
			{
				throw new ArgumentException("Selection collection was empty.");
			}

			if (count == 1)
			{
				Output.Log($"Defaulted to {collection.First()}.");
				return collection.First();
			}

			Output.Log($"Select an item of type {typeof(T).FullName} from this list:");
			var
				i = 0; // Either this should start at one and the .../{count - 1}... part should not have - 1 or we keep it as is.
			foreach (T item in collection)
			{
				Output.Log($"Item ({i}/{count - 1}): {item}");
				getInput:
				Console.Write("Select? Y/N: ");
				var readChar = Console.ReadKey().KeyChar;
				Output.Log();
				if (readChar == 'Y' || readChar == 'y')
				{
					Output.Log($"Selected {item}");
					return item;
				}

				if (readChar == 'N' || readChar == 'n')
				{
					i++;
				}
				else
				{
					Output.ErrorLog("Invalid input.");
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
			for (var i = 0; i < parameterInfoArray.Length; i++)
			{
				ParameterInfo parameter = parameterInfoArray[i];

				if (parameter.IsOptional)
				{
					if (AskYOrNQuestion(
						$"{parameter.Name} is optional, and defaults to {(parameter.DefaultValue is null ? "null" : parameter.DefaultValue)}, do you want to change it?")
					)
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

			if (type.IsEnum)
			{
				return new ImaginaryEnum(type, SelectItem(Enum.GetNames(type)));
			}

			if (type.QualifiesAsPrimitive())
			{
				return new ImaginaryPrimitive(Convert.ChangeType(Console.ReadLine(), type));
			}

			if (type.GetConstructors().Length > 0)
			{
				return new ImaginaryConstructableObject(type, GetConstructorParameters(type));
			}

			return new ImaginaryConstructableObject(type);
		}

		#endregion
	}
}