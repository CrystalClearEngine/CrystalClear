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
		[XmlIgnore] public DirectoryInfo AssetsDirectory;

		[XmlIgnore] public DirectoryInfo BuildDirectory;

		[XmlIgnore] public DirectoryInfo HierarchiesDirectory;

		[XmlIgnore] public DirectoryInfo ProjectDirectory;

		public string ProjectName;

		[XmlIgnore] public DirectoryInfo ScriptsDirectory;

		[XmlIgnore] public DirectoryInfo TempDirectory;
		// TODO: add EditorData-like ProjectData storage for project specific preferences and data.
		
		public string ProjectCrystalClearVersionString
		{
			get => ProjectCrystalClearVersion.ToString();
			set => ProjectCrystalClearVersion = Version.Parse(value);
		}
		
		[XmlIgnore]
		public Version ProjectCrystalClearVersion { get; set; } =
			new Version(0, 0, 0,
				3); // Set the default to 0.0.0.3 since that was the last version of Crystal Clear that did not store the version number.

		[XmlIgnore]
		public FileInfo ProjectFile => new FileInfo($@"{CurrentProject.Path}\{CurrentProject.ProjectName}.crcl");

		public string Path
		{
			get => ProjectDirectory.FullName;
			set => ProjectDirectory = new DirectoryInfo(value);
		}

		// TODO: make paths relative.
		public string ScriptsPath
		{
			get => ScriptsDirectory.FullName;
			set => ScriptsDirectory = new DirectoryInfo(value);
		}

		public string BuildPath // TODO: should add / at end?
		{
			get => BuildDirectory.FullName;
			set => BuildDirectory = new DirectoryInfo(value);
		}

		public string AssetsPath
		{
			get => AssetsDirectory.FullName;
			set => AssetsDirectory = new DirectoryInfo(value);
		}

		public string HierarchyPath
		{
			get => HierarchiesDirectory.FullName;
			set => HierarchiesDirectory = new DirectoryInfo(value);
		}

		public string TempPath
		{
			get => HierarchiesDirectory.FullName;
			set => HierarchiesDirectory = new DirectoryInfo(value);
		}

		#region Project Management

		public static void SaveCurrentProject()
		{
			var xmlSerializer = new XmlSerializer(typeof(ProjectInfo));
			var projectWriter = XmlWriter.Create(CurrentProject.ProjectFile.OpenWrite());

			xmlSerializer.Serialize(projectWriter, CurrentProject);
		}

		public static void NewProject(string projectPath, string projectName)
		{
			DirectoryInfo projectDirectory = Directory.CreateDirectory(projectPath);

			projectDirectory.Create();

			var project = new ProjectInfo
			{
				ProjectDirectory = projectDirectory,
				ProjectName = projectName,
				ProjectCrystalClearVersion = CrystalClearVersion,
			};

			project.ScriptsDirectory = projectDirectory.CreateSubdirectory(@"Scripts");
			project.BuildDirectory = projectDirectory.CreateSubdirectory(@"Build");
			project.AssetsDirectory = projectDirectory.CreateSubdirectory(@"Assets");
			project.HierarchiesDirectory = projectDirectory.CreateSubdirectory(@"Hierarchies");
			project.TempDirectory = projectDirectory.CreateSubdirectory(@"Temp");

			var xmlSerializer = new XmlSerializer(typeof(ProjectInfo));

			using (FileStream fileStream = File.OpenWrite($@"{project.Path}\{project.ProjectName}.crcl"))
			using (var projectWriter = XmlWriter.Create(fileStream, new XmlWriterSettings {Indent = true}))
			{
				xmlSerializer.Serialize(projectWriter, project);
			}

			OpenProject(projectDirectory.FullName);
		}

		public static void OpenProject(string projectPath)
		{
			var projectDirectory = new DirectoryInfo(projectPath);

			if (!IsProject(projectDirectory))
			{
				throw new Exception("No project exists at this location.");
			}

			var xmlSerializer = new XmlSerializer(typeof(ProjectInfo));

			FileInfo[] crystalClearProjectFiles = projectDirectory.GetFiles("*.crcl");

			if (crystalClearProjectFiles.Length > 1)
			{
				Output.Log(
					$"There are multiple Crystal Clear project files in this project folder, defaulting to {crystalClearProjectFiles[0].Name}.");
			}

			using (FileStream fileStream = crystalClearProjectFiles[0].OpenRead())
			using (var reader = XmlReader.Create(fileStream))
			{
				var loadedProject = (ProjectInfo) xmlSerializer.Deserialize(reader);

				if (loadedProject.ProjectCrystalClearVersion < CrystalClearVersion)
				{
					Output.ErrorLog(
						$"{loadedProject.ProjectName} is from an older version of Crystal Clear. (Project version: {loadedProject.ProjectCrystalClearVersion} < Current Crystal Clear Version: {CrystalClearVersion})");
				}

				else if (loadedProject.ProjectCrystalClearVersion > CrystalClearVersion)
				{
					Output.ErrorLog(
						$"{loadedProject.ProjectName} is from a newer version of Crystal Clear. (Project version: {loadedProject.ProjectCrystalClearVersion} > Current Crystal Clear Version: {CrystalClearVersion})");
				}

				CurrentProject = loadedProject;
			}

			Console.Title = $"{CurrentProject.ProjectName} | Crystal Clear Engine {CrystalClearVersion}";
		}

		public static bool IsProject(DirectoryInfo projectDirectory)
		{
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
				CurrentProject.ProjectDirectory.MoveTo(
					Directory.GetParent(CurrentProject.ProjectDirectory.FullName).FullName + @"\" + newName);
			}

			SaveCurrentProject();
		}

		#endregion
	}
}