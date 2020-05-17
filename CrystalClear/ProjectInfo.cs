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
		// TODO: add EditorData-like ProjectData storage for project specific preferences and data.

		[XmlElement]
		public Version ProjectCrystalClearVersion { get; set; } = new Version(0,0,0,3); // Set the default to 0.0.0.3 since that was the last version of Crystal Clear that did not store the version number.

		public string ProjectName;

		[XmlIgnore]
		public DirectoryInfo ProjectDirectory;

		// TODO: why is this even serialized? Can't this just be determied automatically?
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
		public DirectoryInfo BuildDirectory;
		public string BuildPath
		{
			get
			{
				return BuildDirectory.FullName;
			}
			set
			{
				BuildDirectory = new DirectoryInfo(value);
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
				ProjectCrystalClearVersion = CrystalClearVersion,
			};

			project.ScriptsDirectory = projectDirectory.CreateSubdirectory(@"Scripts");
			project.BuildDirectory = projectDirectory.CreateSubdirectory(@"Build");
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
				ProjectInfo loadedProject = (ProjectInfo)xmlSerializer.Deserialize(reader);

				ConsoleColor beforeColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Yellow;
				if (loadedProject.ProjectCrystalClearVersion < CrystalClearVersion)
				{
					Console.WriteLine($"{loadedProject.ProjectName} is from an older version of Crystal Clear. (Project: {loadedProject.ProjectCrystalClearVersion} < Current Crystal Clear Version: {CrystalClearVersion})");
				}

				else if (loadedProject.ProjectCrystalClearVersion > CrystalClearVersion)
				{
					Console.WriteLine($"{loadedProject.ProjectName} is from a newer version of Crystal Clear. (Project: {loadedProject.ProjectCrystalClearVersion} > Current Crystal Clear Version: {CrystalClearVersion})");
				}
				Console.ForegroundColor = beforeColor;

				CurrentProject = loadedProject;
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
