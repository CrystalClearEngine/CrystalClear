using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using static CrystalClear.CrystalClearInformation;
using static CrystalClear.EditorInformation;

namespace CrystalClear
{
	public class ProjectInfo
	{
		// TODO: add EditorData-like ProjectData storage for project specific preferences and data.

		[XmlElement]
		public Version ProjectCrystalClearVersion { get; set; } = new Version(0, 0, 0, 3); // Set the default to 0.0.0.3 since that was the last version of Crystal Clear that did not store the version number.

		public string ProjectName;

		[XmlIgnore]
		public FileInfo ProjectFile => new FileInfo($@"{CurrentProject.Path}\{CurrentProject.ProjectName}.crcl");

		[XmlIgnore]
		public DirectoryInfo ProjectDirectory;

		public string Path
		{
			get => ProjectDirectory.FullName;
			set => ProjectDirectory = new DirectoryInfo(value);
		}

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

		#region Project Management
		public static void SaveCurrentProject()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectInfo));
			XmlWriter projectWriter = XmlWriter.Create(CurrentProject.ProjectFile.OpenWrite());

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
			DirectoryInfo projectDirectory = new DirectoryInfo(projectPath);

			if (!IsProject(projectDirectory.FullName))
			{
				throw new Exception("No project exists at this location.");
			}

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectInfo));

			FileInfo[] crystalClearProjectFiles = projectDirectory.GetFiles("*.crcl");

			if (crystalClearProjectFiles.Length > 1)
			{
				Console.WriteLine($"There are multiple Crystal Clear project files in this project folder, defaulting to {crystalClearProjectFiles[0].Name}.");
			}

			using (FileStream fileStream = crystalClearProjectFiles[0].OpenRead())
			using (XmlReader reader = XmlReader.Create(fileStream))
			{
				ProjectInfo loadedProject = (ProjectInfo)xmlSerializer.Deserialize(reader);

				ConsoleColor beforeColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Yellow;
				if (loadedProject.ProjectCrystalClearVersion < CrystalClearVersion)
				{
					Console.WriteLine($"{loadedProject.ProjectName} is from an older version of Crystal Clear. (Project version: {loadedProject.ProjectCrystalClearVersion} < Current Crystal Clear Version: {CrystalClearVersion})");
				}

				else if (loadedProject.ProjectCrystalClearVersion > CrystalClearVersion)
				{
					Console.WriteLine($"{loadedProject.ProjectName} is from a newer version of Crystal Clear. (Project version: {loadedProject.ProjectCrystalClearVersion} > Current Crystal Clear Version: {CrystalClearVersion})");
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

		public static void ModifyCurrentProject(string newName, bool changeFolderNameToMatch)
		{
			CurrentProject.ProjectFile.Delete();

			CurrentProject.ProjectName = newName;

			if (changeFolderNameToMatch)
			{
				// TODO: unload UserGeneratedContent before this. It won't work otherwise.
				CurrentProject.ProjectDirectory.MoveTo(Directory.GetParent(CurrentProject.ProjectDirectory.FullName).FullName + @"\" + newName);
			}

			SaveCurrentProject();
		}
		#endregion
	}
}
