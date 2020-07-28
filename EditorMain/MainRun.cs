using CrystalClear;
using CrystalClear.HierarchySystem;
using CrystalClear.RuntimeMain;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using System;
using System.Diagnostics;
using System.Reflection;
using static CrystalClear.EditorInformation;

partial class MainClass
{
	private static void Run(ImaginaryHierarchyObject rootHierarchyObject)
	{
		#region Running
		if (compiledAssembly is null)
		{
			Output.ErrorLog("error: code not compiling");
			return;
		}

		Console.Write("Choose a name for the hierarchy: ");
		string hierarchyName = Console.ReadLine();

		Output.Log();

#if DEBUG
		CrystalClearInformation.UserAssemblies = new[] { compiledAssembly, Assembly.GetAssembly(typeof(HierarchyObject)) };

		RuntimeMain.RunWithImaginaryHierarchyObject(new[] { compiledAssembly }, hierarchyName, rootHierarchyObject);

		while (true) ; // In DEBUG mode you cannot exit the run.
#endif

#if RELEASE // TODO: send and recieve path to Hierarchy to use and also use anonymous pipes for communication.
		Process userProcess = new Process();

		userProcess.StartInfo = new ProcessStartInfo(@"E:\dev\crystal clear\RuntimeMain\bin\Debug\netcoreapp3.1\RuntimeMain.exe", CurrentProject.BuildPath + @"\UserGeneratedCode.dll");

		userProcess.Start();

		userProcess.WaitForExit();

		Console.WriteLine($"The program exited with code {userProcess.ExitCode}.");

		userProcess.Dispose();

		Console.WriteLine("Press any key to continue.");

		Console.ReadKey(true);

		Console.Clear();
#endif
#endregion
	}
}