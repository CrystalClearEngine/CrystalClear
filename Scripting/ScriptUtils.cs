using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Scripting
{
	public static class UserSettings
	{
		public static string savePath = Environment.CurrentDirectory + @"\settings";

		public static void SaveSetting(Setting setting)
		{
			string[] lines = File.ReadAllLines(savePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.StartsWith(setting.name)) //This setting should be overwritten as it is the setting we want to change
				{
					File.WriteAllLines(savePath, lines.Where((v, j) => j != i));
					File.AppendAllText(savePath, "\n" + SettingToString(setting));
				}
			}
		}

		internal static string SettingToString(Setting setting)
		{
			return setting.name + ": " + setting.value;
		}

		public struct Setting
		{
			public string name;
			public string value;
		}
	}

	public static class Output
	{
		public static void Log(string str)
		{
			Console.WriteLine(str);
		}
		public static void Log(string str, ConsoleColor bgColor, ConsoleColor fgColor)
		{
			var prevFgColor = Console.ForegroundColor; //Store previous foreground and background color so that we can restore them after writing
			var prevBgColor = Console.BackgroundColor;
			Console.BackgroundColor = bgColor; //Set new colors
			Console.ForegroundColor = fgColor;
			Console.WriteLine(str); //Write string
			Console.BackgroundColor = prevBgColor; //Restore previous colors
			Console.ForegroundColor = prevFgColor;
		}
	}
}
