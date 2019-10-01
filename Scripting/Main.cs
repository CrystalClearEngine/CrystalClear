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
			AppDomain scriptDomain = AppDomain.CreateDomain("ScriptDomain");

			List<string> codeFiles = new List<string>()
			{
				System.IO.File.ReadAllText(@"E:\dev\crystal clear\Scripting\Scripts\Program.cs"),
				System.IO.File.ReadAllText(@"E:\dev\crystal clear\Scripting\Scripts\MyScript2.cs")
			};

			Console.WriteLine(FileCombining.CombineFiles(codeFiles));

			string code =
				FileCombining.CombineFiles(codeFiles);

			//Hardcoded code to compile
			Assembly compiledScript = Compiling.CompileCode(

				code

			);

			if (compiledScript == null)
			{
				Console.ReadLine();
				Environment.Exit(-1);
			}

			/*scriptDomain*/Assembly.Load("Scripts");

			Events.Event.RunStartEvents(Compiling.FindInterfaceEvents(compiledScript));

			Console.ReadLine();
		}
	}
}
