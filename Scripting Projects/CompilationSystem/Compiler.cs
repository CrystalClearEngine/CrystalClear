using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace CrystalClear.CompilationSystem
{
	/// <summary>
	/// Static class for compiler features.
	/// </summary>
	public static class Compiler
	{
		/// <summary>
		/// Compiles C# code files to an assembly. Will in the future likely also support other .net languages!
		/// </summary>
		/// <param name="fileNames">The files to compile.</param>
		/// <returns>The compiled assembly.</returns>
		public static Assembly CompileCode(string[] fileNames)
		{
			using (TempFileCollection temp = new TempFileCollection(Environment.CurrentDirectory))
			using (CSharpCodeProvider csProvider = new CSharpCodeProvider())
			{
				AppDomain.CurrentDomain.ProcessExit += new EventHandler((object sender, EventArgs e) => temp.Delete());

				// Set the options for the compilation.
				CompilerParameters options = new CompilerParameters
				{
					GenerateExecutable = false,
#if DEBUG
					IncludeDebugInformation = true,
					GenerateInMemory = false,
					TempFiles = new TempFileCollection(Environment.CurrentDirectory, true)
#else
					IncludeDebugInformation = false,
					GenerateInMemory = true,
					TempFiles = new TempFileCollection(Environment.CurrentDirectory, false)
#endif
				};

				// The collection of references.
				string[] references =
				{
					@"E:\dev\crystal clear\Scripting Projects\ScriptUtilities\bin\Debug\ScriptUtilities.dll", // The path to the ScriptUtilities dll
					@"E:\dev\crystal clear\Scripting Projects\EventSystem\bin\Debug\EventSystem.dll", // The path to the EventSystem dll
					@"E:\dev\crystal clear\Scripting Projects\HierarchySystem\bin\Debug\HierarchySystem.dll", // The path to the EventSystem dll
					@"E:\dev\crystal clear\Scripting Projects\Standard\bin\Debug\Standard.dll", // The path to the Standard dll
					Assembly.GetExecutingAssembly().Location // The location of the CompilationSystem.
				};

				// Set references for the compiled code.
				options.ReferencedAssemblies.AddRange(references);

				// Compile our code.
				CompilerResults result = csProvider.CompileAssemblyFromFile(options, fileNames);

				// Iterate through all errors and report them to the user.
				foreach (object error in result.Errors)
				{
					Console.WriteLine(error);
				}

				// If we have errors then return null.
				if (result.Errors.HasErrors)
				{
					return null;
				}
				
				// Return our successfull compiled assembly.
				return result.CompiledAssembly;
			}
		}
	}
}