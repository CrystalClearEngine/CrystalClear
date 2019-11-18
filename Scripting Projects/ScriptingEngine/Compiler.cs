using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace CrystalClear.ScriptingEngine
{
	internal static class Compiler
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

				// The collection of references
				string[] references =
				{
					@"E:\dev\crystal clear\Scripting Projects\ScriptUtilities\bin\Debug\ScriptUtilities.dll", // The path to the ScriptUtilities dll
					@"E:\dev\crystal clear\Scripting Projects\EventSystem\bin\Debug\EventSystem.dll", // The path to the EventSystem dll
					@"E:\dev\crystal clear\Scripting Projects\HierarchySystem\bin\Debug\HierarchySystem.dll", // The path to the EventSystem dll
					@"E:\dev\crystal clear\Scripting Projects\Standard\bin\Debug\Standard.dll", // The path to the Standard dll
					Assembly.GetExecutingAssembly().Location // The location of the ScriptingEngine
				};

				// Set references for the compiled code
				options.ReferencedAssemblies.AddRange(references);

				// Compile our code
				CompilerResults result = csProvider.CompileAssemblyFromFile(options, fileNames);

				foreach (object error in result.Errors)
				{
					Console.WriteLine(error);
				}

				if (result.Errors.HasErrors)
				{
					return null;
				}

				return result.CompiledAssembly;
			}
		}
	}
}