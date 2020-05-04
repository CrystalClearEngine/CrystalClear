using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
												CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest)
												, fileName, Encoding.UTF8)).ToList();

				// The collection of references.
				string[] references =
				{
					@"C:\Program Files\dotnet\packs\NETStandard.Library.Ref\2.1.0\ref\netstandard2.1\netstandard.dll",
					@"E:\dev\crystal clear\SerializationSystem\bin\Debug\netstandard2.0\SerializationSystem.dll", // The path to the SerializationSystem dll.
					@"E:\dev\crystal clear\ScriptUtilities\bin\Debug\netstandard2.0\ScriptUtilities.dll", // The path to the ScriptUtilities dll.
					@"E:\dev\crystal clear\EventSystem\bin\Debug\netstandard2.0\EventSystem.dll", // The path to the EventSystem dll.
					@"E:\dev\crystal clear\HierarchySystem\bin\Debug\netstandard2.0\HierarchySystem.dll", // The path to the EventSystem dll.
					@"E:\dev\crystal clear\Standard\bin\Debug\netstandard2.0\Standard.dll", // The path to the Standard dll.
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