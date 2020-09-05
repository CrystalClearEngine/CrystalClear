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

namespace EditorMain
{
	public static partial class MainClass
	{
		// TODO: should make main method that wraps loop around this.
		public static void ParseCommand(string input, ref ImaginaryHierarchyObject rootHierarchyObject, ref ImaginaryHierarchyObject currentSelectedHierarchyObject)
		{
			string[] commandSections = input.Split(' ');

			// Command recognition.
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
					AddHierarchyObject(currentSelectedHierarchyObject, CreateImaginaryHierarchyObject(ref currentSelectedHierarchyObject, SelectItem(HierarchyObjectTypes)), commandSections[1]);
					break;

				case "del":
					DeleteHierarchyObject(currentSelectedHierarchyObject, commandSections.Length < 1
						? GetName(currentSelectedHierarchyObject)
						: commandSections[1]);
					break;

				case "rename":
					Rename(currentSelectedHierarchyObject, commandSections[1]);
					break;

				case "modify":
					Modify(ref currentSelectedHierarchyObject, commandSections.Length >= 2 ? commandSections[1] : null);
					break;

				case "details":
					Details(ref currentSelectedHierarchyObject, commandSections.Length >= 2 ? commandSections[1] : null);
					break;

				case "add":
					AddScript(currentSelectedHierarchyObject, CreateImaginaryScript(SelectItem(ScriptTypes)), commandSections[1]);
					break;

				case "rem":
					RemoveScript(currentSelectedHierarchyObject, commandSections[1]);
					break;

				case "save":
					Save(commandSections[1], rootHierarchyObject);
					break;

				case "load":
					rootHierarchyObject = Load(commandSections[1]);
					currentSelectedHierarchyObject = rootHierarchyObject;
					break;

				case "pack":
					Pack(commandSections[1], rootHierarchyObject);
					break;

				case "unpack":
					rootHierarchyObject = Unpack(commandSections[1]);
					currentSelectedHierarchyObject = rootHierarchyObject;
					break;

				case "list":
					List(ref currentSelectedHierarchyObject, commandSections.Length >= 2 ? commandSections[1] : null);
					break;

				case "select":
					Select(ref currentSelectedHierarchyObject, commandSections.Length >= 2 ? commandSections[1] : null);
					break;

				case "export":
					switch (commandSections[1])
					{
						case "prefab":
							ExportPrefab(currentSelectedHierarchyObject, commandSections[2], commandSections[3]);
							break;

						case "copy":
							ExportHierarchy(currentSelectedHierarchyObject, commandSections[2], commandSections[3]);
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
							var prefab = ImportPrefab(commandSections[2]);
							AddHierarchyObject(currentSelectedHierarchyObject, (ImaginaryHierarchyObject)prefab.CreateInstance(), prefab.Name);
							break;

						case "copy":
							var hierarchy = ImportHierarchy(commandSections[2]);
							AddHierarchyObject(currentSelectedHierarchyObject, (ImaginaryHierarchyObject)hierarchy.CreateInstance(), hierarchy.Name);
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
					Builder.Build(commandSections[1], commandSections[2]);
					break;

				case "run":
					Run(rootHierarchyObject, commandSections[1]);
					break;

				case "exit":
					Environment.Exit(0);
					break;

				default:
					Output.ErrorLog("command error: unknown command");
					break;
			}
#pragma warning restore CA1031 // Do not catch general exception types

			static void Modify(ref ImaginaryHierarchyObject currentSelectedHierarchyObject, string toModify = null)
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
						ref ((ImaginaryEditableObject)hierarchyObjectToModify.ImaginaryObjectBase).EditorData);
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

			static void Details(ref ImaginaryHierarchyObject currentSelectedHierarchyObject, string toDetail = null)
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

			static ImaginaryHierarchyObject CreateImaginaryHierarchyObject(ref ImaginaryHierarchyObject currentSelectedHierarchyObject, Type ofType)
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

			// Lists all HierarchyObjects in the current HierarchyObject's local Hierarchy.
			static void List(ref ImaginaryHierarchyObject currentSelectedHierarchyObject, string toList = null)
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
			static void Select(ref ImaginaryHierarchyObject currentSelectedHierarchyObject, string editorObjectSelectQuery = null)
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

		public static void SetName(ImaginaryHierarchyObject toName, string newName)
		{
			newName = Utilities.EnsureUniqueName(newName, toName.LocalHierarchy.Keys);
			toName.Parent.LocalHierarchy.Remove(toName.Parent.LocalHierarchy
				.First(x => ReferenceEquals(x.Value, toName)).Key);
			toName.Parent.LocalHierarchy.Add(newName, toName);
		}

		public static string GetName(ImaginaryHierarchyObject toName)
		{
			if (toName.Parent is null)
			{
				return string.Empty;
			}

			return toName.Parent.LocalHierarchy.First(x => ReferenceEquals(x.Value, toName)).Key;
		}

		public static void ExportHierarchy(ImaginaryHierarchyObject toExport, string exportPath, string name = null)
		{
			var imaginaryHierarchy = new ImaginaryHierarchy(toExport,
				name is null ? GetName(toExport) : name);
			ImaginaryObjectSerialization.SaveToFile(exportPath, imaginaryHierarchy);
		}

		public static ImaginaryHierarchy ImportHierarchy(string hierarchyPath)
		{
			return ImaginaryObjectSerialization.LoadFromSaveFile<ImaginaryHierarchy>(hierarchyPath);
		}

		public static void ExportPrefab(ImaginaryHierarchyObject toExport, string exportPath, string name)
		{
			var imaginaryHierarchyPrefab = new HierarchyPrefab(toExport, name, exportPath);
			ImaginaryObjectSerialization.SaveToFile(exportPath, imaginaryHierarchyPrefab);
		}

		public static HierarchyPrefab ImportPrefab(string prefabPath)
		{
			var imaginaryHierarchyPrefab = ImaginaryObjectSerialization.LoadFromSaveFile<HierarchyPrefab>(prefabPath);

			imaginaryHierarchyPrefab.PrefabPath = prefabPath;

			return imaginaryHierarchyPrefab;
		}

		public static ImaginaryHierarchyObject Unpack(string path)
		{
			try
			{
				var unpacked = (ImaginaryHierarchyObject)ImaginaryObjectSerialization.UnpackImaginaryObject(path);

				Output.Log($"Successfuly unpacked from location {path}.", ConsoleColor.Black, ConsoleColor.Green);

				return unpacked;
			}
			catch (FileNotFoundException)
			{
				Output.ErrorLog("command error: the file cannot be found");

				return null;
			}
		}

		public static void Pack(string path, ImaginaryHierarchyObject toPack)
		{
			try
			{
				ImaginaryObjectSerialization.PackImaginaryObjectToFile(path, toPack);

				Output.Log($"Successfuly packed to location {path}.", ConsoleColor.Black, ConsoleColor.Green);
			}
			catch (FileNotFoundException)
			{
				Output.ErrorLog("command error: the destination cannot be found");
			}
		}

		public static ImaginaryHierarchyObject Load(string path)
		{
			try
			{
				var loaded = ImaginaryObjectSerialization.LoadFromSaveFile<ImaginaryHierarchyObject>(path);

				Output.Log($"Successfuly loaded from location {path}.", ConsoleColor.Black, ConsoleColor.Green);

				return loaded;
			}
			catch (FileNotFoundException)
			{
				Output.ErrorLog("command error: the file cannot be found");

				return null;
			}
		}

		public static void Save(string path, ImaginaryHierarchyObject rootHierarchyObject)
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

		public static void RemoveScript(ImaginaryHierarchyObject toRemoveFrom, string scriptName)
		{
			toRemoveFrom.AttachedScripts.Remove(scriptName);
			Output.Log($"Script {scriptName} has been removed.");
		}

		public static void AddScript(ImaginaryHierarchyObject toAddTo, ImaginaryScript script, string name)
		{
			name = Utilities.EnsureUniqueName(name, toAddTo.AttachedScripts.Keys);

			toAddTo.AttachedScripts.Add(name, script);

			Output.Log($"Script {name} has been added!");
		}

		public static void Rename(ImaginaryHierarchyObject toRename, string newName)
		{
			if (toRename.Parent is null)
			{
				Output.ErrorLog(
					"command error: currently selected HierarchyObject has no parent and has therefore no name and cannot be renamed.");
				return;
			}

			var oldName = GetName(toRename);
			SetName(toRename, newName);
			Output.Log($"Renamed {oldName} to {newName}.");
		}

		public static void DeleteHierarchyObject(ImaginaryHierarchyObject toDeleteFrom, string nameOfEditorHierarchyObjectToDelete)
		{
			toDeleteFrom.LocalHierarchy.Remove(nameOfEditorHierarchyObjectToDelete);
			Output.Log($"HierarchyObject {nameOfEditorHierarchyObjectToDelete} has been deleted.");
		}

		public static void AddHierarchyObject(ImaginaryHierarchyObject parent, ImaginaryHierarchyObject toAdd, string name)
		{
			name = Utilities.EnsureUniqueName(name, parent.LocalHierarchy.Keys);

			toAdd.Parent = parent;
			parent.LocalHierarchy.Add(name, toAdd);
			Output.Log($"HierarchyObject {name} has been added!");
		}

		public static void Analyze(Assembly assemblyToAnalyze)
		{
			if (assemblyToAnalyze is null)
				return;

			#region Type identification

			var standardAssembly = Assembly.GetAssembly(typeof(ScriptObject));

			// Find all scripts that are present in the compiled assembly.
			ScriptTypes = Script.FindScriptTypesInAssemblies(new[] { assemblyToAnalyze, standardAssembly });

			// Find all HierarchyObject types in the compiled assembly.
			HierarchyObjectTypes = HierarchyObject.FindHierarchyObjectTypesInAssemblies(new[] { assemblyToAnalyze, standardAssembly });

			#endregion
		}

		public static Assembly Compile()
		{
			var compiledAssembly = Compiler.CompileCode(CodeFilePaths);

			// If the compiled assembly is null then something went wrong during compilation (there was probably en error in the code).
			if (compiledAssembly is null)
			{
				// Explain to user that the compilation failed.
				Output.ErrorLog("compilation error: compilation failed :(");
				// TODO: do type identification for Standard regardless.
			}

			Output.Log($"Successfuly built {compiledAssembly.GetName()} at location {compiledAssembly.Location}.",
				ConsoleColor.Black, ConsoleColor.Green);

			return compiledAssembly;
		}
	}
}
