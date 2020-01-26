﻿using CrystalClear.HierarchySystem.Scripting;
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
using System.Linq;

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
			@"E:\dev\crystal clear\Scripting Projects\Scripts\CustomHierarchyObject.cs",
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

		#region Type identification
		// Cache the results.
		Type[] typesInCode = compiledAssembly.GetTypes();

		// Find all scripts that are present in the compiled assembly.
		Type[] scriptTypes = Script.FindScriptTypesInAssembly(compiledAssembly);

		// Find all HierarchyObject types in the compiled assembly.
		List<Type> hierarchyObjectTypes = HierarchyObject.FindHierarchyObjectTypesInAssembly(compiledAssembly).ToList();
		// Add the HierarchyObjects defined in standard HierarchyObjects.
		hierarchyObjectTypes.AddRange(HierarchyObject.FindHierarchyObjectTypesInAssembly(Assembly.GetAssembly(typeof(ScriptObject))));
		#endregion

		#region Editor loop
		// Very basic editor.

		EditorHierarchyObject rootEditorHierarchyObject = new EditorHierarchyObject(null, typeof(HierarchyRoot), null);
		EditorHierarchyObject currentEditorHierarchyObject = rootEditorHierarchyObject;

		LoopEditor:
		string line = Console.ReadLine();

		// Split the command at space that has not been escaped with a \.
		string[] commandSections = Regex.Split(line, @"(?<!\\) ");

		foreach (string section in commandSections)
		{
			// Clean up the \'s from the earlier split operation.
			Regex.Replace(section, @"\\ ", string.Empty);
		}
		try
		{
			switch (commandSections[0])
			{
				case "new":
					if (commandSections.Length > 1)
					{
						New(commandSections[1]);
					}
					else
					{
						// Use default HierarchyObject name if no name is provided.
						New();
					}
					break;

				case "del":
					Delete(commandSections[1]);
					break;

				case "modify":
					Modify();
					break;

				case "add":
					if (commandSections.Length > 1)
					{
						AddScript(commandSections[1]);
					}
					else
					{
						// Use default script name if no name is provided.
						AddScript();
					}
					break;

				case "remove":
					RemoveScript(commandSections[1]);
					break;

				case "save":
					Save(commandSections[1]);
					break;

				case "load":
					Load(commandSections[1]);
					break;

				case "store":
					Store(commandSections[1]);
					break;

				case "unpack":
					Unpack(commandSections[1]);
					break;

				case "select":
					Select(commandSections[1]);
					break;

				case "run":
					goto RunProgram;

				default:
					Console.WriteLine("command error: unknown command");
					break;
			}
		}
#pragma warning disable CA1031 // Do not catch general exception types
		catch (ArgumentNullException)
		{
			Console.WriteLine("command error: incorrect arg");
		}
		catch (IndexOutOfRangeException)
		{
			Console.WriteLine("command error: missing arg");
		}
#pragma warning restore CA1031 // Do not catch general exception types
		goto LoopEditor;
		RunProgram:
		#endregion

		#region Creating and running
		Console.Write("Choose a name for the hierarchy: ");
		HierarchyManager.AddHierarchy(Console.ReadLine(), rootEditorHierarchyObject.CreateInstance(null));
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

		#region Editor Methods
		void Modify()
		{
			currentEditorHierarchyObject.GetModifier();
		}

		void New(string name = null)
		{
			Type hierarchyObjectType = SelectItem(hierarchyObjectTypes);

			// BEGIN CODE FROM HIERARCHYOBJECT'S ADDSCRIPTMANUALLY...
			// TODO debug why it wont work in all instances... there should really be some kind of snippet management software, where if you update the snippet in one place it happens everywhere else too. It should also support overriding certain things where you don't want all copies to be identical. It would really make sense for places where code is reused but classes or methods don't make sense!

			// Replace name with default name if not provided.
			if (name == null)
			{
				name = hierarchyObjectType.Name;
			}

			// Create iterator.
			int i = 1;

			// Repeat while name is already taken in AttatchedScripts.
			while (currentEditorHierarchyObject.AttatchedScripts.ContainsKey(name))
			{
				string OldDuplicateDecorator = $" ({i - 1})";

				// Does the name already contain the OldDuplicateDecorator from a previous attempt?
				if (name.EndsWith(OldDuplicateDecorator))
				{
					name.Remove(name.Length - OldDuplicateDecorator.Length, OldDuplicateDecorator.Length);
				}

				string DuplicateDecorator = $" ({i})";

				name += DuplicateDecorator;

				// Increment iterator.
				i++;
			}

			// END CODE FROM HIERARCHYOBJECT'S ADDSCRIPTMANUALLY...

			currentEditorHierarchyObject.LocalHierarchy.Add(name, new EditorHierarchyObject(currentEditorHierarchyObject, hierarchyObjectType, null));
			Console.WriteLine($"HierarchyObject {name} has been added!");
		}

		void Delete(string nameOfEditorHierarchyObjectToDelete)
		{
			currentEditorHierarchyObject.LocalHierarchy.Remove(nameOfEditorHierarchyObjectToDelete);
		}

		void AddScript(string name = null)
		{
			Type scriptType = SelectItem(scriptTypes);
			object[] constructorParameters = GetConstructorParameters(scriptType);

			// BEGIN CODE FROM HIERARCHYOBJECT'S ADDSCRIPTMANUALLY...

			// Replace name with default name if not provided.
			if (name == null)
			{
				name = scriptType.Name;
			}

			// Create iterator.
			int i = 1;

			// Repeat while name is already taken in AttatchedScripts.
			while (currentEditorHierarchyObject.AttatchedScripts.ContainsKey(name))
			{
				string OldDuplicateDecorator = $" ({i - 1})";

				// Does the name already contain the OldDuplicateDecorator from a previous attempt?
				if (name.EndsWith(OldDuplicateDecorator))
				{
					name.Remove(name.Length - OldDuplicateDecorator.Length, OldDuplicateDecorator.Length);
				}

				string DuplicateDecorator = $" ({i})";

				name += DuplicateDecorator;

				// Increment iterator.
				i++;
			}

			// END CODE FROM HIERARCHYOBJECT'S ADDSCRIPTMANUALLY...

			currentEditorHierarchyObject.AttatchedScripts.Add(name, new EditorScript(scriptType, constructorParameters));

			Console.WriteLine($"Script {name} has been added!");
		}

		void RemoveScript(string name)
		{
			currentEditorHierarchyObject.AttatchedScripts.Remove(name);
			Console.WriteLine($"{name} has been removed.");
		}

		void Save(string path)
		{
			EditorObjectSerialization.SaveToFile(path, rootEditorHierarchyObject);
		}

		void Load(string path)
		{
			rootEditorHierarchyObject = (EditorHierarchyObject)EditorObjectSerialization.LoadFromSaveFile(path, typeof(EditorHierarchyObject));
			currentEditorHierarchyObject = rootEditorHierarchyObject;
		}

		void Store(string path)
		{
			EditorObjectSerialization.StoreToFile(path, rootEditorHierarchyObject);
		}

		void Unpack(string path)
		{
			rootEditorHierarchyObject = (EditorHierarchyObject)EditorObjectSerialization.LoadFromStoreFile(path);
			currentEditorHierarchyObject = rootEditorHierarchyObject;
		}

		void Select(string editorObjectSelectQuery)
		{
			// Does this query start with a backstep?
			if (editorObjectSelectQuery.StartsWith("<"))
			{
				// Get the count of backsteps.
				int backStepCount = editorObjectSelectQuery.TakeWhile((char c) => (c == '<')).Count();

				// Backstep.
				for (int i = 0; i < backStepCount; i++)
				{
					// Perform the backstep by going back to the parent of currentEditorHierarchyObject.
					currentEditorHierarchyObject = currentEditorHierarchyObject.Parent;
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

			currentEditorHierarchyObject = currentEditorHierarchyObject.LocalHierarchy[editorObjectSelectQuery];
		}

		T SelectItem<T>(IEnumerable<T> collection)
		{
			selection:
			// The number of items in the collection.
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
			foreach (var item in collection)
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
				else if(readChar == 'N' || readChar == 'n')
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

		object[] GetConstructorParameters(Type scriptType)
		{
			Console.WriteLine($"Constructor parameter wizard for {scriptType.FullName}.");

			Console.WriteLine("Please select a constructor.");
			ConstructorInfo constructorInfo = SelectItem(scriptType.GetConstructors());
			ParameterInfo[] parameterInfoArray = constructorInfo.GetParameters();

			object[] parameters = new object[parameterInfoArray.Length];

			Console.WriteLine("Now provide values for the different parameters.");
			for (int i = 0; i < parameterInfoArray.Length; i++)
			{
				ParameterInfo parameter = parameterInfoArray[i];
				
				Console.WriteLine($"{parameter.Name}:");
				parameters[i] = CreateObject(parameter.ParameterType);
			}

			Console.WriteLine("Done! This is how it's looking:");
			Console.Write($"new {scriptType.Name} (");
			parameters.ToList().ForEach((object o) => { Console.Write(o.ToString() + ", "); });
			Console.WriteLine("\b\b)");

			return parameters;
		}

		object CreateObject(Type type)
		{
			if (type.IsPrimitive || type == typeof(string) /*|| type.IsAssignableFrom(IEditorSerializeable)*/ )
			{
				return Convert.ChangeType(Console.ReadLine(), type);
			}
			else
				// TODO use ImaginaryObject (pretty much EditorObject, probs gonna rename it) instead of wasting cycles creating an object which is going to be removed only to be created again soon. Will also prevent security vulnerabilities and I am already rambling way too much.
				return Activator.CreateInstance(type, GetConstructorParameters(type));
		}
		#endregion
	}
}