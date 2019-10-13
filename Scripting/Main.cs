using CrystalClear.Scripting.ScriptingEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using CrystalClear.Scripting.EventSystem;
using CrystalClear.Scripting.EventSystem.Events;
using CrystalClear.Scripting.ScriptAttributes;

namespace CrystalClear.Scripting
{
	public static class MainClass
	{
		private static void Main()
		{
			restart:

			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US"); // So that we get relevant exception messages. Who thought it would be a good idea to translate them and NOT LET YOU CHOOSE which language to use.
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

			string[] scriptFilesPaths = new[]
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


			List<Script> scripts = (from exportedType in compiledScript.GetExportedTypes()
				from attribute in exportedType.GetCustomAttributes()
				where attribute is ScriptAttribute
				select new Script(exportedType)).ToList();

			foreach (Script script in scripts)
			{
				foreach (MethodInfo method in script.ScriptType.GetMethods())
				{
					foreach (Attribute attribute in method.GetCustomAttributes())
					{
						if (attribute is SubscribeToAttribute subscribeToAttribute)
						{
							if (Delegate.CreateDelegate(subscribeToAttribute.EventType, script.ScriptInstance, method) is StartEventHandler startEventHandler)
								StartEventClass.StartEvent += startEventHandler;
							if (Delegate.CreateDelegate(subscribeToAttribute.EventType, script.ScriptInstance, method) is ExitEventHandler exitEventHandler)
								ExitEventClass.ExitEvent += exitEventHandler;
						}
					}
				}
			}

			StartEventClass.RaiseStartEvent();

			scripts[0].DynamicallyCallMethod("DynamicallyCallMe");
			scripts[0].DynamicallyCallMethods(new[]{"IToo", "AndMe"});


			Console.ReadLine();


			if (ExitEventClass.RaiseExitEvent().IsCancelled == true)
			{
				goto restart;
			}

			Console.Read();
		}
	}
}
