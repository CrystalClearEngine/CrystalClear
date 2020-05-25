using CrystalClear.ScriptUtilities;
using System.IO;
using System.Reflection;

namespace CrystalClear.CompilationSystem
{
	/// <summary>
	/// Contains methods for building your Crystal Clear application.
	/// </summary>
	public static class Builder
	{
		/// <summary>
		/// Builds your Crystal Clear application.
		/// </summary>
		/// <param name="buildPath">The path to build the EXE and data files to.</param>
		/// <param name="exectutableName">The name of the executable file, without the extension.</param>
		/// <param name="userGeneratedAssemblies">All user generated assemblies that should be included in compilation.</param>
		public static void Build(string buildPath, string exectutableName, Assembly[] userGeneratedAssemblies)
		{
			// The DirectoryInfo for the buildPath.
			DirectoryInfo buildDirectory = new DirectoryInfo(buildPath);
			// Create the path (if it doesn't already exist).
			buildDirectory.Create();

			// Compile the (windows only so far) executable.
			bool success = Compiler.CompileWindowsExecutable(GenerateMainMethodCode(raiseStartEvent: false), userGeneratedAssemblies, buildPath, exectutableName);

			// Stop if the compile was not successful.
			if (!success)
			{ // If the build failed...
				// Output that an error has occured in the compilation.
				Output.ErrorLog("Compilation of executable failed :(, returning.", false);
				// Return from  the method.
				return;
			}

			// Output that the compilation was successful.
			Output.Log($@"Successfuly built {exectutableName} at location {buildPath}\{exectutableName}.exe.", System.ConsoleColor.Black, System.ConsoleColor.Green);
		}

		/// <summary>
		/// A method used by Build to generate the code for the main method.
		/// </summary>
		/// <param name="raiseStartEvent">If the start event should be raised.</param>
		/// <param name="hierarchyToLoadInitially">Optional path to an Hierarchy that should be loaded when the application is run.</param>
		/// <param name="mainClassName">Allows you to set a custom and hopefully more imaginative name than "Program" for your application's main class.</param>
		/// <returns>The generated code.</returns>
		private static string GenerateMainMethodCode(bool raiseStartEvent = true, string hierarchyToLoadInitially = null, string mainClassName = "Program")
		{
			string mainMethodCode =
			"using CrystalClear.RuntimeMain;" +
			$"public static class {mainClassName}" +
			"{" +
				"public static void Main(string[] args)" +
				"{" +
					$"RuntimeMain.Run({(hierarchyToLoadInitially is null ? $"{raiseStartEvent.ToString().ToLower()}" : $"\"{hierarchyToLoadInitially}\", \"Hierarchy\", {raiseStartEvent.ToString().ToLower()}")});" +
				"}" +
			"}";

			return mainMethodCode;
		}
	}
}
