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
			RuntimeInformation.UserAssemblies = new System.Collections.Generic.HashSet<Assembly>
			{
				typeof(ScriptObject).Assembly,
				typeof(Script).Assembly,
				userGeneratedCode,
			};

			RuntimeInformation.UserAssemblies.UnionWith(new[] { Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(HierarchyObject)), Assembly.GetAssembly(typeof(ScriptObject)) });

			RuntimeMain.RunWithImaginaryHierarchyObject(RuntimeInformation.UserAssembliesArray, hierarchyName, rootHierarchyObject);

			RuntimeMain.WaitForStop(10);

			RuntimeInformation.UserAssemblies = null;
			#endregion
		}
	}
}