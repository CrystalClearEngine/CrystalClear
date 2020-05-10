using System;
using System.IO;
using System.Xml;
using static CrystalClear.EditorInformation;
using static CrystalClear.CrystalClearInformation;
using System.Xml.Serialization;

namespace CrystalClear
{
	public class ProjectInfo
	{
		public string ProjectName;

		[XmlIgnore]
		public DirectoryInfo ProjectDirectory;

		public string Path { get => ProjectDirectory.FullName; }

		[XmlIgnore]
		public DirectoryInfo ScriptsDirectory;
		// TODO: make paths relative.
		public string ScriptsPath
		{
			get
			{
				return ScriptsDirectory.FullName;
			}
			set
			{
				ScriptsDirectory = new DirectoryInfo(value);
			}
		}

		[XmlIgnore]
		public DirectoryInfo AssetsDirectory;
		public string AssetsPath
		{
			get
			{
				return AssetsDirectory.FullName;
			}
			set
			{
				AssetsDirectory = new DirectoryInfo(value);
			}
		}

		[XmlIgnore]
		public DirectoryInfo HierarchiesDirectory;
		public string HierarchyPath
		{
			get
			{
				return HierarchiesDirectory.FullName;
			}
			set
			{
				HierarchiesDirectory = new DirectoryInfo(value);
			}
		}

		#region Management
		public static void SaveProject()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectInfo));
			XmlWriter projectWriter = XmlWriter.Create(File.OpenWrite($@"{CurrentProject.Path}\{CurrentProject.ProjectName}.crcl"));

			xmlSerializer.Serialize(projectWriter, CurrentProject);
		}

		public static void NewProject(string projectPath, string projectName)
		{
			DirectoryInfo projectDirectory = Directory.CreateDirectory(projectPath);

			projectDirectory.Create();

			ProjectInfo project = new ProjectInfo()
			{
				ProjectDirectory = projectDirectory,
				ProjectName = projectName,
			};

			project.ScriptsDirectory = projectDirectory.CreateSubdirectory(@"Scripts");
			project.AssetsDirectory = projectDirectory.CreateSubdirectory(@"Assets");
			project.HierarchiesDirectory = projectDirectory.CreateSubdirectory(@"Hierarchies");

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectInfo));

			using (FileStream fileStream = File.OpenWrite($@"{project.Path}\{project.ProjectName}.crcl"))
			using (XmlWriter projectWriter = XmlWriter.Create(fileStream, new XmlWriterSettings() { Indent = true }))
			{
				xmlSerializer.Serialize(projectWriter, project);
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

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectInfo));

			using (FileStream fileStream = File.OpenRead(projectDirectory.GetFiles("*.crcl")[0].FullName))
			using (XmlReader reader = XmlReader.Create(fileStream))
			{
				CurrentProject = (ProjectInfo)xmlSerializer.Deserialize(reader);
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
