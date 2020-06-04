﻿using CrystalClear.ScriptUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static CrystalClear.EditorInformation;

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
			using (FileStream dllStream = File.Create(CurrentProject.BuildPath + @"\UserGeneratedCode.dll"))
			using (FileStream pdbStream = File.Create(CurrentProject.BuildPath + @"\UserGeneratedCode.pdb"))
			{
				List<SyntaxTree> syntaxTrees = (from string fileName in codeFileNames
												select CSharpSyntaxTree.ParseText(File.ReadAllText(fileName),
												CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest)
												, fileName, Encoding.UTF8)).ToList();

				string[] references =
				{
					@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.Runtime.dll",
					@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.Runtime.Extensions.dll",
					@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.Console.dll",
					@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.dll",
					@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\netstandard.dll",
					@"E:\dev\crystal clear\SerializationSystem\bin\Debug\netstandard2.0\SerializationSystem.dll", // The path to the SerializationSystem dll.
					@"E:\dev\crystal clear\ScriptUtilities\bin\Debug\netstandard2.0\ScriptUtilities.dll", // The path to the ScriptUtilities dll.
					@"E:\dev\crystal clear\EventSystem\bin\Debug\netstandard2.0\EventSystem.dll", // The path to the EventSystem dll.
					@"E:\dev\crystal clear\HierarchySystem\bin\Debug\netstandard2.0\HierarchySystem.dll", // The path to the EventSystem dll.
					@"E:\dev\crystal clear\RuntimeMain\bin\Debug\netcoreapp3.1\RuntimeMain.dll", // The path to the RuntimeMain dll.
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

				// Compile.
				EmitResult emitResult = compilation.Emit(dllStream, pdbStream);

				// Report the diagnostics to the user using this handy-dany method.
				ReportDiagnostics(emitResult.Diagnostics.ToArray());

				if (!emitResult.Success)
				{
					return null;
				}
			}
			return Assembly.LoadFrom(CurrentProject.BuildPath + @"\UserGeneratedCode.dll");
		}

		/// <summary>
		/// Compiles C# code to a windows exectuable.
		/// </summary>
		/// <param name="code">The code to compile.</param>
		public static bool CompileWindowsExecutable(string code, Assembly[] userGeneratedAssemblies, string outputPath, string executableName)
		{
			using FileStream exeStream = File.Create($@"{outputPath}\{executableName}.exe");
			using FileStream pdbStream = File.Create($@"{outputPath}\{executableName}.pdb");


			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code
											, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest)
											, encoding: Encoding.UTF8);

			string[] references =
			{
				@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.Runtime.dll",
				@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.Runtime.Extensions.dll",
				@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.Console.dll",
				@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.dll",
				@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\netstandard.dll",
				@"E:\dev\crystal clear\SerializationSystem\bin\Debug\netstandard2.0\SerializationSystem.dll", // The path to the SerializationSystem dll.
				@"E:\dev\crystal clear\ScriptUtilities\bin\Debug\netstandard2.0\ScriptUtilities.dll", // The path to the ScriptUtilities dll.
				@"E:\dev\crystal clear\EventSystem\bin\Debug\netstandard2.0\EventSystem.dll", // The path to the EventSystem dll.
				@"E:\dev\crystal clear\HierarchySystem\bin\Debug\netstandard2.0\HierarchySystem.dll", // The path to the EventSystem dll.
				@"E:\dev\crystal clear\RuntimeMain\bin\Debug\netcoreapp3.1\RuntimeMain.dll", // The path to the RuntimeMain dll.
				@"E:\dev\crystal clear\Standard\bin\Debug\netstandard2.0\Standard.dll", // The path to the Standard dll.
				Assembly.GetExecutingAssembly().Location // The location of the CompilationSystem dll.
			};

			List<MetadataReference> metadataReferences = new List<MetadataReference>();
			foreach (string reference in references)
			{
				metadataReferences.Add(MetadataReference.CreateFromFile(reference));
			}

			foreach (Assembly userGeneratedAssembly in userGeneratedAssemblies)
			{
				metadataReferences.Add(MetadataReference.CreateFromFile(userGeneratedAssembly.Location));
			}

			CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.ConsoleApplication, optimizationLevel: OptimizationLevel.Release);

			CSharpCompilation compilation = CSharpCompilation.Create(
				executableName,
				new[] { syntaxTree },
				metadataReferences,
				options);

			// Compile
			EmitResult emitResult = compilation.Emit(exeStream, pdbStream);

			// Report the diagnostics to the user using this handy-dany method!
			ReportDiagnostics(emitResult.Diagnostics.ToArray());

			return emitResult.Success;
		}

		private static void ReportDiagnostics(Diagnostic[] diagnostics)
		{
			foreach (Diagnostic diagnostic in diagnostics)
			{
				switch (diagnostic.Severity)
				{
					case DiagnosticSeverity.Error:
					case DiagnosticSeverity.Warning:
						Output.ErrorLog(diagnostic.ToString(), diagnostic.Severity == DiagnosticSeverity.Warning);
						break;
					case DiagnosticSeverity.Info:
						Output.Log(diagnostic.ToString());
						break;
					case DiagnosticSeverity.Hidden:
						break;
				}
			}
		}
	}
}