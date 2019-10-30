using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;

namespace CrystalClear.ScriptingEngine
{
	internal static class Compiling
	{
		public static Assembly CompileCode(string[] fileNames)
		{
			using (CSharpCodeProvider csProvider = new CSharpCodeProvider())
			{
				// Set the options for the compilation
				CompilerParameters options = new CompilerParameters
				{
					GenerateExecutable = false,
					IncludeDebugInformation = true,
					TempFiles = new TempFileCollection(Environment.CurrentDirectory, false)
				};

				// Set references for the compiled code
				options.ReferencedAssemblies.Add("\\netstandard2.0\\ScriptRuntime.dll");

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