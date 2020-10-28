using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using static CrystalClear.EditorInformation;

namespace CrystalClear.CompilationSystem
{
	/// <summary>
	///     Static class for compiler features.
	/// </summary>
	public static class Compiler
	{
		private static readonly string[] references =
		{
			@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.Runtime.dll",
			@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.Runtime.Extensions.dll",
			@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.Console.dll",
			@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\System.dll",
			@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\3.1.0\ref\netcoreapp3.1\netstandard.dll",
			@"E:\dev\CrystalClear\SerializationSystem\bin\Debug\netcoreapp3.1\SerializationSystem.dll", // The path to the SerializationSystem dll.
			@"E:\dev\CrystalClear\ScriptUtilities\bin\Debug\netcoreapp3.1\ScriptUtilities.dll", // The path to the ScriptUtilities dll.
			@"E:\dev\CrystalClear\EventSystem\bin\Debug\netcoreapp3.1\EventSystem.dll", // The path to the EventSystem dll.
			@"E:\dev\CrystalClear\HierarchySystem\bin\Debug\netcoreapp3.1\HierarchySystem.dll", // The path to the EventSystem dll.
			@"E:\dev\CrystalClear\RuntimeMain\bin\Debug\netcoreapp3.1\RuntimeMain.dll", // The path to the RuntimeMain dll.
			@"E:\dev\CrystalClear\Standard\bin\Debug\netcoreapp3.1\Standard.dll", // The path to the Standard dll.
			@"E:\dev\CrystalClear\MessageSystem\bin\Debug\netcoreapp3.1\MessageSystem.dll", // The path to the MessageSystem dll.
			@"E:\dev\CrystalClear\CompilationSystem\bin\Debug\netcoreapp3.1\CompilationSystem.dll", // The location of the CompilationSystem dll.
			@"E:\dev\CrystalClear\CrystalClear\bin\Debug\netcoreapp3.1\CrystalClear.dll", // The location of the CrystalClear dll.
		};

		/// <summary>
		///     Compiles C# source code files to an assembly. Will in the future likely also support other .net languages!
		/// </summary>
		/// <param name="codeFileNames">The files to compile.</param>
		/// <returns>Whether the compilation was successful.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
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

				List<MetadataReference> metadataReferences = new List<MetadataReference>();
				foreach (var reference in references)
				{
					metadataReferences.Add(MetadataReference.CreateFromFile(reference));
				}

				var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);


				var compilation = CSharpCompilation.Create(
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
#if RELEASE
			return Assembly.LoadFrom(CurrentProject.BuildPath + @"\UserGeneratedCode.dll"); // Load the assembly and lock the file.
#endif
#if DEBUG
			// TODO: Perhaps change the location property of the assembly after loading it?
			return Assembly.Load(
				File.ReadAllBytes(CurrentProject.BuildPath +
				                  @"\UserGeneratedCode.dll")); // Load the assembly without locking the file.
#endif
		}

		/// <summary>
		///     Compiles C# code to a windows exectuable.
		/// </summary>
		/// <param name="code">The code to compile.</param>
		public static bool CompileWindowsExecutable(string code, Assembly[] userGeneratedAssemblies, string outputPath,
			string executableName)
		{
			using FileStream exeStream = File.Create($@"{outputPath}\{executableName}.exe");
			using FileStream pdbStream = File.Create($@"{outputPath}\{executableName}.pdb");


			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code
				, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest)
				, encoding: Encoding.UTF8);

			List<MetadataReference> metadataReferences = new List<MetadataReference>();
			foreach (var reference in references)
			{
				metadataReferences.Add(MetadataReference.CreateFromFile(reference));
			}

			foreach (Assembly userGeneratedAssembly in userGeneratedAssemblies)
			{
				metadataReferences.Add(MetadataReference.CreateFromFile(userGeneratedAssembly.Location));
			}

			var options = new CSharpCompilationOptions(OutputKind.ConsoleApplication,
				optimizationLevel: OptimizationLevel.Release);

			var compilation = CSharpCompilation.Create(
				executableName,
				new[] {syntaxTree},
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