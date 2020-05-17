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
		/// <param name="codeFileNames">The files to compile.</param>
		/// <returns>The compiled assembly.</returns>
		public static Assembly CompileCode(string[] codeFileNames)
		{
			// TODO: Maybe it should be called UserGenerated only?
			using (FileStream dllStream = File.Create(WorkingPath + @"\UserGeneratedCode.dll"))
			using (FileStream pdbStream = File.Create(WorkingPath + @"\UserGeneratedCode.pdb"))
			{
				List<SyntaxTree> syntaxTrees = (from string fileName in codeFileNames
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
					Assembly.GetExecutingAssembly().Location // The location of the CompilationSystem dll.
				};

				List<MetadataReference> metadataReferences = new List<MetadataReference>();
				foreach (string reference in references)
				{
					metadataReferences.Add(MetadataReference.CreateFromFile(reference));
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

		/// <summary>
		/// Compiles C# code to an exectuable.
		/// </summary>
		/// <param name="code">The code to compile.</param>
		public static bool CompileExecutable(string code, Assembly[] userGeneratedAssemblies, string outputPath, string executableName)
		{
			using (FileStream exeStream = File.Create($@"{outputPath}\{executableName}.exe"))
			using (FileStream pdbStream = File.Create($@"{outputPath}\{executableName}.pdb"))
			{
				SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code
												, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest)
												, encoding: Encoding.UTF8);

				// The collection of references.
				string[] references =
				{
					@"C:\Program Files\dotnet\packs\NETStandard.Library.Ref\2.1.0\ref\netstandard2.1\netstandard.dll", // The path to the netstandard dll. This may be unique for each system.
					@"E:\dev\crystal clear\SerializationSystem\bin\Debug\netstandard2.0\SerializationSystem.dll", // The path to the SerializationSystem dll.
					@"E:\dev\crystal clear\ScriptUtilities\bin\Debug\netstandard2.0\ScriptUtilities.dll", // The path to the ScriptUtilities dll.
					@"E:\dev\crystal clear\EventSystem\bin\Debug\netstandard2.0\EventSystem.dll", // The path to the EventSystem dll.
					@"E:\dev\crystal clear\HierarchySystem\bin\Debug\netstandard2.0\HierarchySystem.dll", // The path to the EventSystem dll.
					@"E:\dev\crystal clear\Standard\bin\Debug\netstandard2.0\Standard.dll", // The path to the Standard dll.
					@"E:\dev\crystal clear\RuntimeMain\bin\Debug\netcoreapp3.1\RuntimeMain.dll", // The path to the RuntimeMain dll.
					Assembly.GetExecutingAssembly().Location // The location of the CompilationSystem dll.
				};

				List<MetadataReference> metadataReferences = new List<MetadataReference>();
				foreach (string reference in references)
				{
					metadataReferences.Add(MetadataReference.CreateFromFile(reference));
				}

				foreach (var userGeneratedAssembly in userGeneratedAssemblies)
				{
					metadataReferences.Add(MetadataReference.CreateFromFile(userGeneratedAssembly.Location));
				}

				CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.ConsoleApplication, optimizationLevel: OptimizationLevel.Release);

				CSharpCompilation compilation = CSharpCompilation.Create(
					executableName,
					new[] { syntaxTree },
					metadataReferences,
					options);

				EmitResult emitResult = compilation.Emit(exeStream, pdbStream);

				foreach (Diagnostic diagnostic in emitResult.Diagnostics)
				{
					Console.WriteLine(diagnostic.ToString());
				}

				return emitResult.Success;
			}
		}
	}
}