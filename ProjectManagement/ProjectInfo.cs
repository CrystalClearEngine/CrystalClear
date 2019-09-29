using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement
{
	public class ProjectInfo
	{
		public ProjectInfo() { }

		public ProjectInfo(string name)
		{
			this.name = name;
		}

		public string name;
		public string currentProjectVersion = CrystalClear.CrystalClearInformation.crystalClearVersion.ToString();
		public Version GetVersion()
		{
			return new Version(currentProjectVersion);
		}
	}
}
