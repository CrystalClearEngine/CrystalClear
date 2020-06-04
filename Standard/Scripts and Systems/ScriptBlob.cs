using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.ScriptUtilities;
using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using NLua;
using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using CrystalClear.Standard.Events;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using System.Reflection;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using CrystalClear.HierarchySystem;

namespace CrystalClear.Standard.Scripts
{
	[IsScript]
	// TODO: add support for Editable scripts!
	//[Editable(nameof(Editor), nameof(Creator))]
	public class ScriptBlob
		: IDisposable
	{
		//private static void Editor(ref EditorData data)
		//{
			
		//}

		//private static object Creator(EditorData data)
		//{
		//	throw new NotImplementedException();
		//}

		public enum ScriptingLanguage
		{
			Python,
			Lua,
			CSharpScript,
		}

		public ScriptBlob(ScriptingLanguage scriptingLanguage, string code, ScriptEventBase executeOn = null)
		{
			language = scriptingLanguage;

			this.code = code;

			executeOn?.Subscribe((ScriptEventHandler)RunCode);

			this.executeOn = executeOn;
		}

		ScriptEventBase executeOn;

		ScriptingLanguage language;

		string code;

		ScriptEngine pythonEngine;

		Lua lua;

		Script cSharpScript;

		[OnFrameUpdate]
		public void RunCode()
		{
			switch (language)
			{
				case ScriptingLanguage.Python:

					if (pythonEngine is null)
					{
						pythonEngine = Python.CreateEngine();
					}

					pythonEngine.Execute(code);

					break;

				case ScriptingLanguage.Lua:

					if (lua is null)
					{
						lua = new Lua();

						lua.LoadCLRPackage();
					}

					lua.DoString(code);

					break;

				case ScriptingLanguage.CSharpScript:

					if (cSharpScript is null)
					{
						//string[] references =
						//{
						//	@"E:\dev\crystal clear\SerializationSystem\bin\Debug\netstandard2.0\SerializationSystem.dll", // The path to the SerializationSystem dll.
						//	@"E:\dev\crystal clear\ScriptUtilities\bin\Debug\netstandard2.0\ScriptUtilities.dll", // The path to the ScriptUtilities dll.
						//	@"E:\dev\crystal clear\EventSystem\bin\Debug\netstandard2.0\EventSystem.dll", // The path to the EventSystem dll.
						//	@"E:\dev\crystal clear\HierarchySystem\bin\Debug\netstandard2.0\HierarchySystem.dll", // The path to the EventSystem dll.
						//	@"E:\dev\crystal clear\RuntimeMain\bin\Debug\netcoreapp3.1\RuntimeMain.dll", // The path to the RuntimeMain dll.
						//	@"E:\dev\crystal clear\Standard\bin\Debug\netstandard2.0\Standard.dll", // The path to the Standard dll.
						//};

						//List<MetadataReference> metadataReferences = new List<MetadataReference>();
						//foreach (string reference in references)
						//{
						//	metadataReferences.Add(MetadataReference.CreateFromFile(reference));
						//}

						Assembly[] assembliesToLoad = new Assembly[]
						{
							Assembly.GetAssembly(typeof(StartEvent)),
							Assembly.GetAssembly(typeof(ScriptEventBase)),
							Assembly.GetAssembly(typeof(HierarchyObject)),
							Assembly.GetAssembly(typeof(FrameUpdateEvent)),
						};

						cSharpScript = CSharpScript.Create(code, ScriptOptions.Default.AddReferences(assembliesToLoad)
							.AddImports("System"));
					}

					cSharpScript.RunAsync();

					break;
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls.

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					executeOn.Unsubscribe((ScriptEventHandler)RunCode);

					lua.Dispose();
				}

				disposedValue = true;
			}
		}

		~ScriptBlob()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
