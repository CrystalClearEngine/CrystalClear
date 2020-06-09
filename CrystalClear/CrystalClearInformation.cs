using System;

namespace CrystalClear
{
	public static class CrystalClearInformation // TODO add a default CrystalClearException for all other to derive from.
	{
		private static readonly Version crystalClearVersion = new Version(0, 0, 0, 5);
		public static Version CrystalClearVersion { get => crystalClearVersion; }

		public static string WorkingPath => $@"{Environment.CurrentDirectory}\"; // TODO make constant, also maybe rename to WorkPath?

		public enum ExitCodes
		{
			Error = -1,
			ForceClose = 0,
			Close = 1,
		}
	}

	public static class EditorInformation
	{
		public static ProjectInfo CurrentProject;
	}
}