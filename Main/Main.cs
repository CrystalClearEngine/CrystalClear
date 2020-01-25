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
					New(commandSections[1]);
					break;

				case "del":
					Delete(commandSections[1]);
					break;

				case "modify":
					Modify();
					break;

				case "add":
					AddScript(commandSections[1]);
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
					Console.WriteLine("command error");
					break;
			}
		}
		catch (ArgumentNullException)
		{
			Console.WriteLine("command error");
		}
		catch (IndexOutOfRangeException)
		{
			Console.WriteLine("command error");
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

		void Modify()
		{
			currentEditorHierarchyObject.GetModifier();
		}

		void New(string name)
		{
			Type typeOfHierarchyObjectToAdd = SelectItem(hierarchyObjectTypes);
			currentEditorHierarchyObject.LocalHierarchy.Add(name, new EditorHierarchyObject(currentEditorHierarchyObject, typeOfHierarchyObjectToAdd, null));
		}

		void Delete(string nameOfEditorHierarchyObjectToDelete)
		{
			currentEditorHierarchyObject.LocalHierarchy.Remove(nameOfEditorHierarchyObjectToDelete);
		}

		void AddScript(string name)
		{
			Type scriptType = SelectItem(scriptTypes);
			object[] constructorParameters = GetConstructorParameters(scriptType);
			currentEditorHierarchyObject.AttatchedScripts.Add(name, new EditorScript(scriptType, constructorParameters));
		}

		void RemoveScript(string name)
		{
			currentEditorHierarchyObject.AttatchedScripts.Remove(name);
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
			Console.WriteLine($"Select an item of type {typeof(T).FullName} from this list:");
			foreach (var item in collection)
			{
				Console.WriteLine($"Item: {item.ToString()}");
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
	}
}