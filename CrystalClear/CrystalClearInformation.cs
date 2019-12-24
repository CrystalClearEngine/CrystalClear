using System;

namespace CrystalClear
{
	public static class CrystalClearInformation
	{
		private static readonly Version crystalClearVersion = new Version(0, 0, 0, 2);
		public static Version CrystalClearVersion { get => crystalClearVersion; }

		public static string WorkingPath => $@"{Environment.CurrentDirectory}\";
	}
}