using System;
using System.Reflection;

namespace Scripting
{
	public static class MainClass
	{
		static void Main()
		{
			for (int i = 0; i < 50; i++)
			{
				//Hardcoded code to compile
				Assembly compiledScript = ScriptingEngine.Compiling.CompileCode(

					System.IO.File.ReadAllText(@"E:\dev\crystal clear\Scripting\Scripts\Program.cs")

				);

				ScriptingEngine.Compiling.FindTypes(compiledScript);
			}
			Console.ReadLine();
		}
	}
}
