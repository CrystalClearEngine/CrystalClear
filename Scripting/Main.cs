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
							object instance = Activator.CreateInstance(script.ScriptType);

							if (Delegate.CreateDelegate(subscribeToAttribute.EventType, instance, method) is StartEventHandler startEventHandler)
								StartEventClass.StartEvent += startEventHandler;
							if (Delegate.CreateDelegate(subscribeToAttribute.EventType, instance, method) is ExitEventHandler exitEventHandler)
								ExitEventClass.ExitEvent += exitEventHandler;
						}
					}
				}
			}

			StartEventClass.RaiseStartEvent();


			Console.ReadLine();

			ExitEventClass.RaiseExitEvent();

			Console.Read();
		}
	}
}
