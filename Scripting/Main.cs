using System;
using System.Reflection;

namespace Scripting
{
	public static class MainClass
	{
		static void Main()
		{
			//Hardcoded code to compile
			Assembly compiledScript = Compiling.CompileCode(

				System.IO.File.ReadAllText(@"E:\dev\crystal clear\program")

			);

			Compiling.FindTypes(compiledScript);

			Console.ReadLine();
		}
	}
}
