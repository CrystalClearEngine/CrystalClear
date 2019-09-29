using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;

namespace ProjectManagement
{
	static class ProjectManagementConsoleInterface
	{
		static string workInPath = Directory.GetCurrentDirectory();

		static void Main(string[] args)
		{
			if (args.Length >= 1)
			{
				foreach (var arg in args)
				{
					GetInput(arg);
				}
			}

			Console.WriteLine("Crystal Clear Engine project management interface");
			while (true)
			{
				GetInput();
			}
		}

		static void GetInput(string input = null)
		{
			if (input == null)
				input = Console.ReadLine();
			string[] inputArray = input.Split(' ');
			switch (inputArray[0])
			{
				case "new":
					//if (inputArray.Length >= 2)
					//	NewProject(inputArray[1], inputArray[2]);
					//else
						NewProject(inputArray[1]);
					break;
				case "use":
					LoadProject(inputArray[1]);
					break;
				case "del":
					DeleteProject(inputArray[1]);
					break;
				case "lst":
					//if (inputArray.Length >= 2)
					//	NewProject(inputArray[1], inputArray[2]);
					//else
					ListProjects();
					break;
				case "in":
					workInPath = inputArray[1];
					break;
				default:
					break;
			}
		}

		private static void ListProjects()
		{
			foreach (string folderPath in Directory.GetDirectories(workInPath))
			{
				if (IsProject(folderPath))
				{
					Console.WriteLine(GetProjectName(folderPath) + " - " + folderPath);
				}
			}
		}

		private static bool IsProject(string path)
		{
			foreach (var file in Directory.GetFiles(path))
			{
				if (file.EndsWith(".crystalcore"))
				{
					return true;
				}
			}
			return false;
		}

		private static string GetProjectName(string path)
		{
			string crystalCorePath = null;
			foreach (var file in Directory.GetFiles(path))
			{
				if (file.EndsWith(".crystalcore"))
				{
					crystalCorePath = file;
				}
			}

			XmlSerializer xs = new XmlSerializer(typeof(ProjectInfo));
			using (var sr = new StreamReader(crystalCorePath))
			{
				ProjectInfo projectInfo = (ProjectInfo)xs.Deserialize(sr);
				return projectInfo.name;
			}
		}

		private static void DeleteProject(string name, string path = null)
		{
			Console.WriteLine("Press 'y' to proceed deleting");
			if (Console.ReadKey().KeyChar != 'y')
			{
				Console.WriteLine("Cancelled");
				return;
			}


			string folderPath = workInPath + @"\" + name;
			if (path != null)
				folderPath = path + @"\" + name;

			if (!IsProject(folderPath))
			{
				Console.WriteLine("The folder selected is not a project folder.");
				return;
			}

			DirectoryInfo di = new DirectoryInfo(folderPath);

			foreach (FileInfo file in di.GetFiles())
			{
				file.Delete();
			}
			foreach (DirectoryInfo dir in di.GetDirectories())
			{
				dir.Delete(true);
			}

			di.Delete();
		}

		static void NewProject(string name, string path = null)
		{
			ProjectInfo projectInfo = new ProjectInfo(name);
			string folderPath = workInPath + @"\" + name;
			if (path != null)
				folderPath = path + @"\" + name;


			Directory.CreateDirectory(folderPath);
			XmlSerializer xs = new XmlSerializer(typeof(ProjectInfo));
			TextWriter tw = new StreamWriter(folderPath + @"\" + name + ".crystalcore");
			xs.Serialize(tw, projectInfo);
			tw.Dispose();
		}

		static void LoadProject(string projectName = null, string path = null)
		{
			if (projectName == null && path == null)
			{
				return;
			}
		}
	}
}
