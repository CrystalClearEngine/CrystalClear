using CrystalClear;
using CrystalClear.HierarchySystem;
using CrystalClear.RuntimeMain;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Reflection;

partial class MainClass
{
	private static void Run(ImaginaryHierarchyObject rootHierarchyObject)
	{
		#region Running
		if (compiledAssembly is null)
		{
			Output.ErrorLog("error: cannot run if code did not compile");
			return;
		}

		Console.Write("Choose a name for the hierarchy: ");
		string hierarchyName = Console.ReadLine();

		Output.Log();

		RuntimeInformation.UserAssemblies = new[] { compiledAssembly, Assembly.GetAssembly(typeof(HierarchyObject)), Assembly.GetAssembly(typeof(ScriptObject)) };

		RuntimeMain.RunWithImaginaryHierarchyObject(new[] { compiledAssembly }, hierarchyName, rootHierarchyObject);

		while (RuntimeMain.IsRunning)
		{
			if (Console.ReadKey(true).Key == ConsoleKey.Escape)
			{
				RuntimeMain.Stop();
			}
		}

		RuntimeInformation.UserAssemblies = null;
		#endregion
	}
}