using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.ScriptUtilities;
using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;

namespace CrystalClear.Standard.Scripts
{
	[IsScript]
	// TODO: add support for Editable scripts!
	//[Editable(nameof(Editor), nameof(Creator))]
	public class ScriptBlob : IDisposable
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

		public ScriptBlob(ScriptingLanguage scriptingLanguage, string code, ScriptEvent executeOn = null)
		{
			language = scriptingLanguage;

			this.code = code;

			executeOn?.Subscribe((ScriptEventHandler)RunCode);

			this.executeOn = executeOn;
		}

		ScriptEvent executeOn;

		ScriptingLanguage language;

		string code;

		[OnStartEvent]
		public void RunCode()
		{
			switch (language)
			{
				case ScriptingLanguage.Python:
					ScriptEngine engine = Python.CreateEngine();
					engine.Execute(code);
					break;
				case ScriptingLanguage.Lua:
					break;
				case ScriptingLanguage.CSharpScript:
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
