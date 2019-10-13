using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;

namespace CrystalClear.Scripting.ScriptingEngine
{
	internal static class Compiling
	{
		public static Assembly CompileCode(string[] fileNames)
		{
			using (CSharpCodeProvider csProvider = new CSharpCodeProvider())
			{
				CompilerParameters options = new CompilerParameters
				{
					GenerateExecutable = false,
					//GenerateInMemory = true,
					IncludeDebugInformation = true,
					OutputAssembly = "Scripts",
					TempFiles = new TempFileCollection(Environment.CurrentDirectory, false)
				};

				options.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

				// Compile our code
				CompilerResults result;
				result = csProvider.CompileAssemblyFromFile(options, fileNames);

				if (result.Errors.HasErrors)
				{
					foreach (object error in result.Errors) Console.WriteLine(error);
					return null;
				}

				return result.CompiledAssembly;
			}
		}
	}
}