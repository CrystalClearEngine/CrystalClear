using CrystalClear;
using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.RuntimeMain;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Reflection;

namespace EditorMain
{
	partial class MainClass
	{
		private static void Run(ImaginaryHierarchyObject rootHierarchyObject, string hierarchyName, Assembly userGeneratedCode)
		{
			#region Running
			RuntimeInformation.UserAssemblies = new[]
			{
				typeof(ScriptObject).Assembly,
				typeof(Script).Assembly,
				userGeneratedCode,
			};

			RuntimeMain.RunWithImaginaryHierarchyObject(RuntimeInformation.UserAssemblies, hierarchyName, rootHierarchyObject);

			RuntimeMain.WaitForStop(10);

			RuntimeInformation.UserAssemblies = null;
			#endregion
		}
	}
}