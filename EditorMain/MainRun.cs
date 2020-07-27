using CrystalClear;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using System;
using System.Diagnostics;
using static CrystalClear.EditorInformation;

partial class MainClass
{
	private static void Run(ImaginaryHierarchyObject rootHierarchyObject)
	{
		#region Running
		Console.Write("Choose a name for the hierarchy: ");
		string hierarchyName = Console.ReadLine();

		Output.Log();

		Process userProcess = new Process();

		userProcess.StartInfo = new ProcessStartInfo(@"E:\dev\crystal clear\RuntimeMain\bin\Debug\netcoreapp3.1\RuntimeMain.exe", CurrentProject.BuildPath + @"\UserGeneratedCode.dll");

		userProcess.Start();

		userProcess.WaitForExit();

		Console.WriteLine($"The program exited with code {userProcess.ExitCode}.");

		userProcess.Dispose();

		Console.WriteLine("Press any key to continue.");

		Console.ReadKey(true);

		Console.Clear();
		#endregion
	}
}