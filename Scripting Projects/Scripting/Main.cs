using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using CrystalClear.Scripting.EventSystem;
using CrystalClear.Scripting.EventSystem.Events;
using CrystalClear.Scripting.ScriptAttributes;
using CrystalClear.Scripting.ScriptingEngine;

namespace CrystalClear.Scripting
{
	public static class MainClass
	{
		private static void Main()
		{
			string[] scriptFilesPaths =
			{
				@"E:\dev\crystal clear\Scripting\Scripts\Program.cs"
			};

			//Hardcoded code to compile
			Assembly compiledScript = Compiling.CompileCode(
				scriptFilesPaths
			);

			if (compiledScript == null)
			{
				Console.WriteLine("Compilation failed (compiled assembly is null :( )");
				Console.ReadLine();
				Environment.Exit(-1);
			}

			Script[] scripts = Script.FindScripts(compiledScript);

			foreach (Script script in scripts)
			{
				script.SubscribeAllEvents();
			}
		}
	}
}