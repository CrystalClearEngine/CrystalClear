using CrystalClear.ScriptUtilities;
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
				// Store a list of all syntax trees generated from the provided code files.
				List<SyntaxTree> syntaxTrees = (from string fileName in codeFileNames
												select CSharpSyntaxTree.ParseText(File.ReadAllText(fileName),
												CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest)
												, fileName, Encoding.UTF8)).ToList();

				// The collection of references for the compiled code to use.
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

				// Store these references as metadataReferences to be useful.
				List<MetadataReference> metadataReferences = new List<MetadataReference>();
				foreach (string reference in references)
				{
					metadataReferences.Add(MetadataReference.CreateFromFile(reference));
				}

				// The compilation options to use.
				CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

				// The CSharpCompilation instance that will compile all of the code.
				CSharpCompilation compilation = CSharpCompilation.Create(
					"UserGeneratedCode", // Create an assembly called UserGeneratedCode...
					syntaxTrees, // That is using the generated syntax trees...
					metadataReferences, // And the set references...
					options); // With the set options.

				// Emit (compile) using the CSharpCompilation instance to the streams and store the result.
				EmitResult emitResult = compilation.Emit(dllStream, pdbStream);

				// Report the diagnostics to the user using this handy-dany method.
				ReportDiagnostics(emitResult.Diagnostics.ToArray());

				// If the emit was unsuccessful...
				if (!emitResult.Success)
				{
					// Return null as there can't be any assembly generated to return.
					return null;
				}
			}
			// Load the assembly from it's location and then return it. I admit this could've been done inside the previous using statements and using the stream there, but this is clearer and will not be such a hot-path anyways.
			return Assembly.LoadFrom(WorkingPath + @"\UserGeneratedCode.dll");
		}

		/// <summary>
		/// Compiles C# code to a windows exectuable.
		/// </summary>
		/// <param name="code">The code to compile.</param>
		public static bool CompileWindowsExecutable(string code, Assembly[] userGeneratedAssemblies, string outputPath, string executableName)
		{
			using FileStream exeStream = File.Create($@"{outputPath}\{executableName}.exe");
			using FileStream pdbStream = File.Create($@"{outputPath}\{executableName}.pdb");

			// Store a list of all syntax trees generated from the provided code files.
			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code
											, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest)
											, encoding: Encoding.UTF8);

			// The collection of references for the compiled code to use.
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

			// Store these references as metadataReferences to be useful.
			List<MetadataReference> metadataReferences = new List<MetadataReference>();
			foreach (string reference in references)
			{
				metadataReferences.Add(MetadataReference.CreateFromFile(reference));
			}

			// Add the user generated assemblies as references in the metadataReferences list.
			foreach (Assembly userGeneratedAssembly in userGeneratedAssemblies)
			{
				metadataReferences.Add(MetadataReference.CreateFromFile(userGeneratedAssembly.Location));
			}

			// The compilation options to use.
			CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.ConsoleApplication, optimizationLevel: OptimizationLevel.Release);

			// The CSharpCompilation instance that will compile all of the code.
			CSharpCompilation compilation = CSharpCompilation.Create(
				executableName, // Create an assembly named (for now) the same as the executable.
				new[] { syntaxTree }, // That is using the generated syntax trees...
				metadataReferences, // And the set references...
				options); // With the set options.

			// Emit (compile) using the CSharpCompilation instance to the streams and store the result.
			EmitResult emitResult = compilation.Emit(exeStream, pdbStream);

			// Report the diagnostics to the user using this handy-dany method!
			ReportDiagnostics(emitResult.Diagnostics.ToArray());

			// Return whether or not the emit was successful. 
			return emitResult.Success;
		}

		private static void ReportDiagnostics(Diagnostic[] diagnostics)
		{
			// Iterate ovet the diagnostics
			foreach (Diagnostic diagnostic in diagnostics)
			{
				switch (diagnostic.Severity)
				{
					case DiagnosticSeverity.Error: // If the severity is that of an error...
					case DiagnosticSeverity.Warning: //  Or that of an error...
													 // Then output log it as an error. The severity depends on whether  it is an error or just a warning.
						Output.ErrorLog(diagnostic.ToString(), diagnostic.Severity == DiagnosticSeverity.Warning);
						break;
					case DiagnosticSeverity.Info: // if the severity is just info...
												  // Simply output log info.
						Output.Log(diagnostic.ToString());
						break;
					case DiagnosticSeverity.Hidden: // If this type of diagnostic is hidden...
						break; // We want to ignore hidden diagnostics.
				}
			}
		}
	}
}