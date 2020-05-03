using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using static CrystalClear.EditorInformation;
using static CrystalClear.CrystalClearInformation;

namespace CrystalClear
{
	[DataContract]
	public class ProjectInfo
	{
		[DataMember]
		public string ProjectName;

		[DataMember]
		public DirectoryInfo ProjectDirectory;

		public string Path { get => ProjectDirectory.FullName; }

		[DataMember]
		public string ScriptsPath { get; set; }

		[DataMember]
		public string AssetsPath { get; set; }

		[DataMember]
		public string HierarchiesPath { get; set; }

		#region Management
		public static void SaveProject()
		{
			DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ProjectInfo));
			XmlWriter projectWriter = XmlWriter.Create(File.OpenWrite($@"{CurrentProject.Path}\{CurrentProject.ProjectName}.crcl"));

			dataContractSerializer.WriteObject(projectWriter, CurrentProject);
		}

		public static void NewProject(string projectPath, string projectName)
		{
			DirectoryInfo projectDirectory = Directory.CreateDirectory(projectPath);

			projectDirectory.Create();

			projectDirectory.CreateSubdirectory(@"Scripts");
			projectDirectory.CreateSubdirectory(@"Assets");
			projectDirectory.CreateSubdirectory(@"Hierarchies");

			ProjectInfo project = new ProjectInfo()
			{
				ProjectDirectory = projectDirectory,
				ProjectName = projectName,
			};
			project.ScriptsPath = projectPath + @"\Scripts";
			project.AssetsPath = projectPath + @"\Assets";
			project.HierarchiesPath = projectPath + @"\Hierarchies";

			DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ProjectInfo));

			using (FileStream fileStream = File.OpenWrite($@"{project.Path}\{project.ProjectName}.crcl"))
			using (XmlWriter projectWriter = XmlWriter.Create(fileStream, new XmlWriterSettings() { Indent = true }))
			{
				dataContractSerializer.WriteObject(projectWriter, project);
			}

			OpenProject(projectDirectory.FullName);
		}

		public static void OpenProject(string projectPath)
		{
			DirectoryInfo projectDirectory = Directory.CreateDirectory(projectPath);

			if (!IsProject(projectDirectory.FullName))
			{
				throw new Exception("No project exists at this location.");
			}

			DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ProjectInfo));

			using (FileStream fileStream = File.OpenRead(projectDirectory.GetFiles("*.crcl")[0].FullName))
			using (XmlReader reader = XmlReader.Create(fileStream))
			{
				CurrentProject = (ProjectInfo)dataContractSerializer.ReadObject(reader);
			}

			Console.Title = $"{CurrentProject.ProjectName} | Crystal Clear Engine {CrystalClearVersion}";
		}

		public static bool IsProject(string path)
		{
			DirectoryInfo projectDirectory = Directory.CreateDirectory(path);
			if (!projectDirectory.Exists)
			{
				return false;
			}

			if (projectDirectory.GetFiles("*.crcl").Length == 0)
			{
				return false;
			}

			return true;
		}
		#endregion
	}
}
