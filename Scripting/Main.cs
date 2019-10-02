using CrystalClear.Scripting.ScriptingEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CrystalClear.Scripting
{
	public static class MainClass
	{
		private static void Main()
		{
			List<string> scriptFilesPaths = new List<string>()
			{
				@"E:\dev\crystal clear\Scripting\Scripts\Program.cs",
				@"E:\dev\crystal clear\Scripting\Scripts\MyScript2.cs"
			};

			//Hardcoded code to compile
			Assembly compiledScript = Compiling.CompileCode(

				scriptFilesPaths.ToArray()
			);

			if (compiledScript == null)
			{
				Console.ReadLine();
				Environment.Exit(-1);
			}

			Events.Event.RunStartEvents(Compiling.FindInterfaceEvents(compiledScript));

			Console.ReadLine();
		}
	}
}
