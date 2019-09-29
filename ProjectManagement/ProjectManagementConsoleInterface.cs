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
					if (inputArray.Length >= 2)
						NewProject(inputArray[1], inputArray[2]);
					else
						NewProject(inputArray[1]);
					break;
				case "use":
					LoadProject(inputArray[1]);
					break;
				case "in":
					workInPath = inputArray[1];
					break;
				default:
					break;
			}
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
