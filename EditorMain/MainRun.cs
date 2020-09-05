using CrystalClear;
using CrystalClear.HierarchySystem;
using CrystalClear.RuntimeMain;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using System;
using System.Reflection;

namespace EditorMain
{
	partial class MainClass
	{
		private static void Run(ImaginaryHierarchyObject rootHierarchyObject, string hierarchyName)
		{
			#region Running

			RuntimeMain.RunWithImaginaryHierarchyObject(RuntimeInformation.UserAssemblies, hierarchyName, rootHierarchyObject);

			RuntimeMain.WaitForStop(10);

			RuntimeInformation.UserAssemblies = null;
			#endregion
		}
	}
}