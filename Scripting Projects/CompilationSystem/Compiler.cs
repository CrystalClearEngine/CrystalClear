using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static CrystalClear.CrystalClearInformation;

namespace CrystalClear.CompilationSystem
{
	/// <summary>
	/// Static class for compiler features.
	/// </summary>
	public static class Compiler
	{
		/// <summary>
		/// Compiles C# source code files to an assembly. Will in the future likely also support other .net languages!
		/// </summary>
		/// <param name="fileNames">The files to compile.</param>
		/// <returns>The compiled assembly.</returns>
		public static Assembly CompileCode(string[] fileNames)
		{
			// TODO: Maybe it should be called UserGenerated only?
			using (FileStream dllStream = File.Create(WorkingPath + @"\UserGeneratedCode.dll"))
			using (FileStream pdbStream = File.Create(WorkingPath + @"\UserGeneratedCode.pdb"))
			{
				List<SyntaxTree> syntaxTrees = (from string fileName in fileNames
												select CSharpSyntaxTree.ParseText(File.ReadAllText(fileName),
											  CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest))).ToList();

				// The collection of references.
				string[] references =
				{
					@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll",
					@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll",
					@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll",
					@"E:\dev\crystal clear\Scripting Projects\ScriptUtilities\bin\Debug\ScriptUtilities.dll", // The path to the ScriptUtilities dll.
					@"E:\dev\crystal clear\Scripting Projects\EventSystem\bin\Debug\EventSystem.dll", // The path to the EventSystem dll.
					@"E:\dev\crystal clear\Scripting Projects\HierarchySystem\bin\Debug\HierarchySystem.dll", // The path to the EventSystem dll.
					@"E:\dev\crystal clear\Scripting Projects\Standard\bin\Debug\Standard.dll", // The path to the Standard dll.
					Assembly.GetExecutingAssembly().Location // The location of the CompilationSystem.
				};

				List<MetadataReference> metadataReferences = new List<MetadataReference>();
				foreach (string item in references)
				{
					metadataReferences.Add(MetadataReference.CreateFromFile(item));
				}

				CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

				CSharpCompilation compilation = CSharpCompilation.Create(
					"UserGeneratedCode",
					syntaxTrees,
					metadataReferences,
					options);

				EmitResult emitResult = compilation.Emit(dllStream, pdbStream);

				foreach (Diagnostic diagnostic in emitResult.Diagnostics)
				{
					Console.WriteLine(diagnostic.ToString());
				}

				if (!emitResult.Success)
				{
					return null;
				}
			}
			return Assembly.LoadFrom(WorkingPath + @"\UserGeneratedCode.dll");
		}
	}
}