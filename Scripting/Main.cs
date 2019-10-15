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
			restart:

			Thread.CurrentThread.CurrentCulture =
				CultureInfo.CreateSpecificCulture(
					"en-US"); // So that we get relevant exception messages. Who thought it would be a good idea to translate them and NOT LET YOU CHOOSE which language to use.
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

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
				foreach (Event @event in script.GetEvents())
				{
					@event.Subscribe(script.ScriptInstance);
				}

			StartEventClass.RaiseStartEvent();

			//scripts[0].DynamicallyCallMethod("DynamicallyCallMe");
			//scripts[0].DynamicallyCallMethods(new[] {"IToo", "AndMe"});

			Console.ReadLine();


			if (ExitEventClass.RaiseExitEvent().IsCancelled) goto restart;

			Console.Read();
		}
	}
}