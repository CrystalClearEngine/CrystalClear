using System;
using System.IO;
using System.Xml.Serialization;

namespace ProjectManagement
{
	internal static class ProjectManagementConsoleInterface
	{
		private static string _workInPath = Directory.GetCurrentDirectory();

		private static void Main(string[] args)
		{
			if (args.Length >= 1)
				foreach (var arg in args)
					GetInput(arg);

			Console.WriteLine("Crystal Clear Engine project management interface");
			while (true) GetInput();
		}

		private static void GetInput(string input = null)
		{
			if (input == null) input = Console.ReadLine();

			var inputArray = input.Split(' ');
			switch (inputArray[0])
			{
				case "new":
					NewProject(inputArray[1]);
					break;
				case "use":
					LoadProject(inputArray[1]);
					break;
				case "del":
					DeleteProject(inputArray[1]);
					break;
				case "lst":
					ListProjects();
					break;
				case "in":
					_workInPath = inputArray[1];
					break;
			}
		}

		private static void ListProjects()
		{
			foreach (var folderPath in Directory.GetDirectories(_workInPath))
				if (IsProject(folderPath))
					Console.WriteLine(GetProjectName(folderPath) + " - " + folderPath);
		}

		private static bool IsProject(string path)
		{
			foreach (var file in Directory.GetFiles(path))
				if (file.EndsWith(".crystalcore"))
					return true;
			return false;
		}

		private static string GetProjectName(string path)
		{
			string crystalCorePath = null;
			foreach (var file in Directory.GetFiles(path))
				if (file.EndsWith(".crystalcore"))
					crystalCorePath = file;

			XmlSerializer xs = new XmlSerializer(typeof(ProjectInfo));
			using (StreamReader sr = new StreamReader(crystalCorePath))
			{
				ProjectInfo projectInfo = (ProjectInfo) xs.Deserialize(sr);
				return projectInfo.Name;
			}
		}

		private static void DeleteProject(string name)
		{
			Console.WriteLine("Press 'y' to proceed deleting");
			if (Console.ReadKey().KeyChar != 'y')
			{
				Console.WriteLine("Cancelled");
				return;
			}


			var folderPath = _workInPath + @"\" + name;

			if (!IsProject(folderPath))
			{
				Console.WriteLine("The folder selected is not a project folder.");
				return;
			}

			DirectoryInfo di = new DirectoryInfo(folderPath);

			foreach (FileInfo file in di.GetFiles()) file.Delete();
			foreach (DirectoryInfo dir in di.GetDirectories()) dir.Delete(true);

			di.Delete();
		}

		private static void NewProject(string name)
		{
			ProjectInfo projectInfo = new ProjectInfo(name);
			var folderPath = _workInPath + @"\" + name;


			Directory.CreateDirectory(folderPath);
			XmlSerializer xs = new XmlSerializer(typeof(ProjectInfo));
			TextWriter tw = new StreamWriter(folderPath + @"\" + name + ".crystalcore");
			xs.Serialize(tw, projectInfo);
			tw.Dispose();
		}

		private static void LoadProject(string projectName)
		{
		}
	}
}