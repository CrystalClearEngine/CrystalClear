using System;
using System.Reflection;

namespace CrystalClear
{
	public static class CrystalClearInformation // TODO add a default CrystalClearException for all other to derive from.
	{
		public static Version CrystalClearVersion { get; } = new Version(0, 0, 0, 5);

		public static string WorkingPath => $@"{Environment.CurrentDirectory}\"; // TODO make constant, also maybe rename to WorkPath?

		public enum ExitCodes
		{
			Error = -1,
			ForceClose = 0,
			Close = 1,
		}
	}

	public static class RuntimeInformation
	{
		public static Assembly[] UserAssemblies { get; set; }
	}

	public static class EditorInformation
	{
		public static ProjectInfo CurrentProject;
	}
}