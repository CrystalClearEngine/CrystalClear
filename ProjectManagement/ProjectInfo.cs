using System;
using System.IO;
using System.Xml.Serialization;
using CrystalClear;

namespace CrystalClear.ProjectManagement
{
	internal class ProjectInfo
	{
		public string CurrentProjectVersion = CrystalClearInformation.CrystalClearVersion.ToString();
		public string Name;

		public string ProjectPath;

		public ProjectInfo()
		{
		}

		public ProjectInfo(FileInfo file)
		{
			XmlSerializer xs = new XmlSerializer(typeof(ProjectInfo));
			using (StreamReader sr = new StreamReader(file.FullName))
			{
				ProjectInfo projectInfo = (ProjectInfo) xs.Deserialize(sr);
			}
		}

		public ProjectInfo(string name)
		{
			Name = name;
		}

		public Version GetVersion()
		{
			return new Version(CurrentProjectVersion);
		}

		public static void CreateProject(string name)
		{
		}

		public void DeleteProject()
		{
		}
	}
}