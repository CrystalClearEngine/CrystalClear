using CrystalClear.ScriptUtilities;
using System.IO;
using System.Reflection;

namespace CrystalClear.CompilationSystem
{
	public static class Builder
	{
		public static void Build(string buildPath, string exectutableName, Assembly[] userGeneratedAssemblies)
		{
			DirectoryInfo buildDirectory = new DirectoryInfo(buildPath);
			buildDirectory.Create();

			if (!Compiler.CompileExecutable(GenerateMainMethodCode(startDefaultEvents: false), userGeneratedAssemblies, buildPath, exectutableName))
			{
				// If the build failed...
				Output.ErrorLog("Compilation of executable failed :(, returning.");
				return;
			}

			Output.Log($@"Successfully built {exectutableName} at location {buildPath}\{exectutableName}.exe.", System.ConsoleColor.Black, System.ConsoleColor.Green);
		}

		private static string GenerateMainMethodCode(bool startDefaultEvents, string hierarchyToLoadInitially = null, string mainClassName = "Program")
		{
			string mainMethodCode =
			"using CrystalClear.RuntimeMain;" +
			$"public static class {mainClassName}" +
			"{" +
			"	public static void Main(string[] args)" +
			"	{" +
			$"		RuntimeMain.Run({(hierarchyToLoadInitially is null ? string.Empty : hierarchyToLoadInitially)});" +
			"	}" +
			"}";

			return mainMethodCode;
		}
	}
}
