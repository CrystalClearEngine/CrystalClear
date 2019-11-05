using System;
using System.Reflection;
using CrystalClear.ScriptingEngine;

public static class MainClass
{
	private static void Main()
	{
		string[] scriptFilesPaths =
		{
			@"E:\dev\crystal clear\Scripting Projects\Scripts\Program.cs"
		};

		//Hardcoded code to compile
		Assembly compiledScript = Compiler.CompileCode(
			scriptFilesPaths
		);

		if (compiledScript == null)
		{
			Console.WriteLine("Compilation failed (compiled assembly is null)");
			Console.ReadLine();
			Environment.Exit(-1);
		}

		Script[] scripts = Script.FindScriptsInAssembly(compiledScript);

		foreach (Script script in scripts) script.SubscribeAllEvents();

		scripts[0].DynamicallyCallMethod("OnStart");

		Console.Read();
	}
}