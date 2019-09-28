﻿using System;
using System.Reflection;

namespace Scripting
{
	public static class MainClass
	{
		static void Main()
		{
			//AppDomain scriptDomain = AppDomain.CreateDomain("ScriptDomain");

			//Hardcoded code to compile
			Assembly compiledScript = ScriptingEngine.Compiling.CompileCode(

				System.IO.File.ReadAllText(@"E:\dev\crystal clear\Scripting\Scripts\Program.cs")

			);

			//scriptDomain.Load(compiledScript.GetName());

			ScriptingEngine.Compiling.FindTypes(compiledScript);
			Console.ReadLine();
		}
	}
}
