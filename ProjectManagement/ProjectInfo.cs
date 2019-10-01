using System;
using System.IO;
using System.Xml.Serialization;

namespace ProjectManagement
{
	public class ProjectInfo
	{
		public ProjectInfo() { }

		public ProjectInfo(FileInfo file)
		{
			XmlSerializer xs = new XmlSerializer(typeof(ProjectInfo));
			using (var sr = new StreamReader(file.FullName))
			{
				ProjectInfo projectInfo = (ProjectInfo)xs.Deserialize(sr);
			}
		}

		public ProjectInfo(string name)
		{
			this.name = name;
		}

		public string projectPath;
		public string name;
		public string currentProjectVersion = CrystalClear.CrystalClearInformation.crystalClearVersion.ToString();
		public Version GetVersion()
		{
			return new Version(currentProjectVersion);
		}

		public static void CreateProject(string name)
		{

		}

		public void DeleteProject()
		{

		}
	}
}
