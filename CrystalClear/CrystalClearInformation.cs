using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using SFML.Window;

namespace CrystalClear
{
	public static class CrystalClearInformation // TODO add a default CrystalClearException for all other to derive from.
	{
		public static Version CrystalClearVersion { get; } = new Version(0, 0, 0, 6);

		public static string WorkingPath => $@"{Environment.CurrentDirectory}\"; // TODO make constant, also maybe rename to WorkPath?

		public enum ExitCodes
		{
			Error = -1,
			ForceClose = 0,
			Close = 1,
		}

		public static Assembly[] UserAssemblies;
	}

	public static class RuntimeInformation
	{
		public static Assembly[] UserAssemblies = Array.Empty<Assembly>();
	}

	public static class EditorInformation
	{
		public static ProjectInfo CurrentProject;

		public static Window MainEditorWindow;

		public static string[] CodeFilePaths;

		public static Type[] ScriptTypes = Array.Empty<Type>();

		public static Type[] HierarchyObjectTypes = Array.Empty<Type>();
	}
}