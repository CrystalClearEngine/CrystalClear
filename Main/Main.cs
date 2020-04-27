using CrystalClear.CompilationSystem;
using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.RuntimeMain;
using CrystalClear.SerializationSystem;
using CrystalClear.Standard.Events;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

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
		// Store the Types.
		Type[] typesInCode = null;

		// Find all scripts that are present in the compiled assembly.
		Type[] scriptTypes = null;

		// Find all HierarchyObject types in the compiled assembly.
		List<Type> hierarchyObjectTypes = null;

		string[] codeFilePaths = Directory.GetFiles(@"E:\dev\crystal clear\Scripting Projects\Scripts", "*.cs");

		Assembly compiledAssembly;

		// Compile our code.
		Compile();

		FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(@"E:\dev\crystal clear\Scripting Projects\Scripts", "*.cs");
		fileSystemWatcher.Changed += (object _, FileSystemEventArgs _1) => { codeFilePaths = Directory.GetFiles(@"E:\dev\crystal clear\Scripting Projects\Scripts", "*.cs");  Compile(); };
		fileSystemWatcher.EnableRaisingEvents = true;
		#endregion

		#region Editor loop
		// Very basic editor.

		ImaginaryHierarchyObject rootHierarchyObject = new ImaginaryHierarchyObject(null, typeof(HierarchyRoot));
		ImaginaryHierarchyObject currentSelectedHierarchyObject = rootHierarchyObject;

		LoopEditor:
		Console.WriteLine();

		// Gather input.
		string line = Console.ReadLine();

		// Split the command at any space.
		string[] commandSections = line.Split(' ');

		try
		{
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
					DeleteHierarchyObject(commandSections[1]);
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

				case "run":
					goto RunProgram;

				case "exit":
					Environment.Exit(0);
					break;

				default:
					Console.WriteLine("command error: unknown command");
					break;
			}
		}
#pragma warning disable CA1031 // Do not catch general exception types
		catch (ArgumentNullException ex)
		{
			Console.WriteLine($"command error: incorrect arg ({ex.Message})");
		}
		catch (IndexOutOfRangeException ex)
		{
			Console.WriteLine($"command error: missing arg ({ex.Message})");
		}
		catch (NotImplementedException ex)
		{
			Console.WriteLine($"command error: command not implemented ({ex.Message})");
		}
		catch (NotSupportedException ex)
		{
			Console.WriteLine($"command error: not supported ({ex.Message})");
		}
#pragma warning restore CA1031 // Do not catch general exception types
		goto LoopEditor;
		#endregion

		#region Running
		RunProgram:

		Console.Write("Choose a name for the hierarchy: ");
		string hierarchyName = Console.ReadLine();

		Console.WriteLine();

		RuntimeMain.SubscribeAll(compiledAssembly);										
		RuntimeMain.Run(hierarchyName, rootHierarchyObject);
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
			compiledAssembly = Compiler.CompileCode(codeFilePaths);

			// If the compiled assembly is null then something went wrong during compilation (there was probably en error in the code).
			if (compiledAssembly is null)
			{
				// Explain to user that the compilation failed.
				Console.WriteLine("compilation error: compilation failed :( (compiled assembly is null)");
				// TODO: do type identification for Standard regardless.
				return false;
			}

			Console.WriteLine($"Successfully built {compiledAssembly.GetName()} at location {compiledAssembly.Location}.");

			#region Type identification
			// Store the Types.
			typesInCode = compiledAssembly.GetTypes();

			// Find all scripts that are present in the compiled assembly.
			scriptTypes = Script.FindScriptTypesInAssembly(compiledAssembly);

			// Find all HierarchyObject types in the compiled assembly.
			hierarchyObjectTypes = HierarchyObject.FindHierarchyObjectTypesInAssembly(compiledAssembly).ToList();
			// Add the HierarchyObjects defined in standard HierarchyObjects.
			hierarchyObjectTypes.AddRange(HierarchyObject.FindHierarchyObjectTypesInAssembly(Assembly.GetAssembly(typeof(ScriptObject))));
			#endregion

			return true;
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
					Console.WriteLine($"command error: no HierarchyObject named {toModify} can be found!");
					return;
				}
				hierarchyObjectToModify = currentSelectedHierarchyObject.LocalHierarchy[toModify];
			}

			if (AskYOrNQuestion($"Do you want to change the name of the HierarchyObject? Name = {GetName(hierarchyObjectToModify)}"))
			{
				SetName(hierarchyObjectToModify, AskQuestion("Write the new name"));
			}

			if (hierarchyObjectToModify.UsesEditor())
			{
				EditableSystem.OpenEditor(hierarchyObjectToModify.GetConstructionType(), ref hierarchyObjectToModify.EditorData);
			}
			else
			{
				hierarchyObjectToModify.ConstructionParameters = GetConstructorParameters(hierarchyObjectToModify.GetConstructionType());
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
					Console.WriteLine($"command error: no HierarchyObject named {toDetail} can be found!");
					return;
				}
				hierarchyObjectToViewDetailsOf = currentSelectedHierarchyObject.LocalHierarchy[toDetail];
			}

			Console.WriteLine($"Details for {hierarchyObjectToViewDetailsOf}:");

			Console.WriteLine($"Name: {toDetail}");

			Console.WriteLine($"Type: {hierarchyObjectToViewDetailsOf.GetConstructionType().FullName}");

			if (hierarchyObjectToViewDetailsOf.UsesConstructorParameters())
			{
				Console.WriteLine("This HierarchyObject uses constructor parameters to be created.");

				Console.WriteLine($"Parameter count: {hierarchyObjectToViewDetailsOf.ConstructionParameters.Length}");

				Console.Write("Parameters: (");
				bool first = true;
				foreach (ImaginaryObject parameter in hierarchyObjectToViewDetailsOf.ConstructionParameters)
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
			else
			{
				Console.WriteLine("This HierarchyObject uses an Editor to be created and modified.");

				Console.WriteLine($"EditorData count: {hierarchyObjectToViewDetailsOf.EditorData.Count}");

				Console.Write("EditorData: (");
				bool first = true;
				foreach (KeyValuePair<string, string> data in hierarchyObjectToViewDetailsOf.EditorData)
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
		}

		// TODO: Make this support generics (generic HierarchyObjects) will also probably require a change to ImaginaryHierarchyObject. (Script equivalents too so they can support generic Scripts!)
		void NewHierarchyObject(string name = null)
		{
			Type hierarchyObjectType = SelectItem(hierarchyObjectTypes);

			if (name is null)
			{
				name = CrystalClear.Utilities.EnsureUniqueName(hierarchyObjectType.Name, currentSelectedHierarchyObject.LocalHierarchy.Keys);
			}
			else if (currentSelectedHierarchyObject.LocalHierarchy.ContainsKey(name))
			{
				name = CrystalClear.Utilities.EnsureUniqueName(name, currentSelectedHierarchyObject.LocalHierarchy.Keys);
			}

			currentSelectedHierarchyObject.LocalHierarchy.Add(name, CreateImaginaryHierarchyObject(hierarchyObjectType));
			Console.WriteLine($"HierarchyObject {name} has been added!");

			ImaginaryHierarchyObject CreateImaginaryHierarchyObject(Type ofType)
			{
				if (ofType.IsEditable(out _))
				{
					EditorData editorData = EditorData.GetEmpty();
					EditableSystem.OpenEditor(ofType, ref editorData);
					return new ImaginaryHierarchyObject(currentSelectedHierarchyObject, ofType, editorData);
				}
				else if (ofType.GetConstructors().Length > 0)
				{
					return new ImaginaryHierarchyObject(currentSelectedHierarchyObject, ofType, GetConstructorParameters(ofType));
				}
				else
				{
					return new ImaginaryHierarchyObject(currentSelectedHierarchyObject, ofType);
				}
			}
		}

		void DeleteHierarchyObject(string nameOfEditorHierarchyObjectToDelete)
		{
			currentSelectedHierarchyObject.LocalHierarchy.Remove(nameOfEditorHierarchyObjectToDelete);
			Console.WriteLine($"HierarchyObject {nameOfEditorHierarchyObjectToDelete} has been deleted.");
		}

		void Rename(string newName)
		{
			if (currentSelectedHierarchyObject.Parent is null)
			{
				Console.WriteLine("command error: currently selected HierarchyObject has no parent and has therefore no name and cannot be renamed.");
				return;
			}
			string oldName = GetName(currentSelectedHierarchyObject);
			SetName(currentSelectedHierarchyObject, newName);
			Console.WriteLine($"Renamed {oldName} to {newName}.");
		}

		void AddScript(string name = null)
		{
			Type scriptType = SelectItem(scriptTypes);

			if (name is null)
			{
				name = CrystalClear.Utilities.EnsureUniqueName(scriptType.Name, currentSelectedHierarchyObject.AttatchedScripts.Keys);
			}
			else if (currentSelectedHierarchyObject.AttatchedScripts.ContainsKey(name))
			{
				name = CrystalClear.Utilities.EnsureUniqueName(name, currentSelectedHierarchyObject.AttatchedScripts.Keys);
			}

			currentSelectedHierarchyObject.AttatchedScripts.Add(name, CreateImaginaryScript(scriptType));

			Console.WriteLine($"Script {name} has been added!");

			ImaginaryScript CreateImaginaryScript(Type ofType)
			{
				if (ofType.IsEditable(out _))
				{
					EditorData editorData = EditorData.GetEmpty();
					EditableSystem.OpenEditor(ofType, ref editorData);
					return new ImaginaryScript(ofType, editorData);
				}
				else if (ofType.GetConstructors().Length > 0)
				{
					return new ImaginaryScript(ofType, GetConstructorParameters(ofType));
				}
				else
				{
					return new ImaginaryScript(ofType);
				}
			}
		}

		void RemoveScript(string name)
		{
			currentSelectedHierarchyObject.AttatchedScripts.Remove(name);
			Console.WriteLine($"Script {name} has been removed.");
		}

		void Save(string path)
		{
			try
			{
				ImaginaryObjectSerialization.SaveToFile(path, rootHierarchyObject);
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (FileNotFoundException)
			{
				Console.WriteLine("command error: the file cannot be found");
			}
		}

		void Load(string path)
		{
			try
			{
				rootHierarchyObject = ImaginaryObjectSerialization.LoadFromSaveFile<ImaginaryHierarchyObject>(path);
				currentSelectedHierarchyObject = rootHierarchyObject;
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("command error: the file cannot be found");
			}
		}

		void Pack(string path)
		{
			try
			{
				ImaginaryObjectSerialization.PackHierarchyToFile(path, rootHierarchyObject);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("command error: the file cannot be found");
			}
		}

		void Unpack(string path)
		{
			try
			{
				rootHierarchyObject = ImaginaryObjectSerialization.UnpackHierarchyFromFile(path);
				currentSelectedHierarchyObject = rootHierarchyObject;
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("command error: the file cannot be found");
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
				Console.WriteLine(name);
			}
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
						Console.WriteLine($"error: {currentSelectedHierarchyObject} does not have a parent. Reverting the select.");
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
				Console.WriteLine("error: backsteps ('<') cannot be located anywhere else in the query other than at the start.");
			}

			if (!currentSelectedHierarchyObject.LocalHierarchy.ContainsKey(editorObjectSelectQuery))
			{
				Console.WriteLine($"error: the requested HierarchyObject doesn't exist. Name = {editorObjectSelectQuery}");
				currentSelectedHierarchyObject = initiallySelected;
				return;
			}

			currentSelectedHierarchyObject = currentSelectedHierarchyObject.LocalHierarchy[editorObjectSelectQuery];
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
			toName.Parent.LocalHierarchy.Remove(toName.Parent.LocalHierarchy.First(x => ReferenceEquals(x.Value, toName)).Key);
			toName.Parent.LocalHierarchy.Add(newName, toName);
		}

		bool AskYOrNQuestion(string question)
		{
			retry:
			Console.Write(question + ": ");

			switch (Console.ReadKey().KeyChar)
			{
				case 't':
				case 'y':
					Console.WriteLine();
					return true;

				case 'f':
				case 'n':
					Console.WriteLine();
					return false;

				default:
					Console.WriteLine("Invalid!");
					goto retry;
			}
		}

		string AskQuestion(string question)
		{
			Console.Write(question + ": ");
			string response = Console.ReadLine();
			Console.WriteLine();
			return response;
		}

		T SelectItem<T>(IEnumerable<T> collection)
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
				Console.WriteLine($"Defaulted to {collection.First()}.");
				return collection.First();
			}

			Console.WriteLine($"Select an item of type {typeof(T).FullName} from this list:");
			int i = 0; // Either this should start at one and the .../{count - 1}... part should not have - 1 or we keep it as is.
			foreach (T item in collection)
			{
				Console.WriteLine($"Item ({i}/{count - 1}): {item.ToString()}");
				getInput:
				Console.Write("Select? Y/N: ");
				char readChar = Console.ReadKey().KeyChar;
				Console.WriteLine();
				if (readChar == 'Y' || readChar == 'y')
				{
					Console.WriteLine($"Selected {item.ToString()}");
					return item;
				}
				else if (readChar == 'N' || readChar == 'n')
				{
					i++;
					continue;
				}
				else
				{
					Console.WriteLine("Invalid input.");
					goto getInput;
				}
			}
			Console.WriteLine("An item needs to be selected!");
			goto selection;
		}

		ImaginaryObject[] GetConstructorParameters(Type type)
		{
			Console.WriteLine($"Constructor parameter wizard for {type.FullName}.");

			Console.WriteLine("Please select a constructor.");
			ConstructorInfo constructorInfo = SelectItem(type.GetConstructors());
			ParameterInfo[] parameterInfoArray = constructorInfo.GetParameters();

			ImaginaryObject[] parameters = new ImaginaryObject[parameterInfoArray.Length];

			Console.WriteLine("Now provide values for the different parameters.");
			for (int i = 0; i < parameterInfoArray.Length; i++)
			{
				ParameterInfo parameter = parameterInfoArray[i];

				Console.WriteLine($"{parameter.Name}:");
				parameters[i] = CreateImaginaryObject(parameter.ParameterType);
			}

			// TODO: fix this for when no parameters are provided.
			Console.WriteLine("Done! This is how it's looking:");
			Console.Write($"new {type.Name} (");
			parameters.ToList().ForEach((ImaginaryObject iO) => { Console.Write(iO.ToString() + ", "); });
			Console.WriteLine("\b\b)");

			return parameters;
		}

		ImaginaryObject CreateImaginaryObject(Type ofType)
		{
			if (ofType.IsEditable(out _))
			{
				EditorData editorData = EditorData.GetEmpty();
				EditableSystem.OpenEditor(ofType, ref editorData);
				return new ImaginaryObject(ofType, editorData);
			}
			else if (ImaginaryPrimitive.QualifiesAsImaginaryPrimitive(ofType) || ofType.GetInterface(nameof(IConvertible)) != null || ofType.GetInterface(nameof(IFormattable)) != null)
			{
				return new ImaginaryPrimitive(Convert.ChangeType(Console.ReadLine(), ofType));
			}
			else if (ofType.GetConstructors().Length > 0)
			{
				return new ImaginaryObject(ofType, GetConstructorParameters(ofType));
			}
			else
			{
				return new ImaginaryObject(ofType);
			}
		}
		#endregion
	}
}