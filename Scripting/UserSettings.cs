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

		private static readonly bool areUserSettingsSetup;
		public static bool AreUserSettingsSetup => areUserSettingsSetup;

		public static void SaveSetting(string name, string value) => SaveSetting(new Setting(name, value));
		public static void SaveSetting(Setting setting)
		{
			string[] lines = File.ReadAllLines(savePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.StartsWith(setting.name)) //This setting should be overwritten as its name matches that of the setting we want to change
				{
					File.WriteAllLines(savePath, lines.Where((v, j) => j != i)); //Delete setting
				}
			}

			File.AppendAllText(savePath, "\n" + SettingToString(setting)); //Write setting
		}

		public static void DeleteSetting(Setting setting) => DeleteSetting(setting.name);
		public static void DeleteSetting(string name)
		{
			string[] lines = File.ReadAllLines(savePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.StartsWith(name)) //This setting should be deleted as it is matches the name of the setting we want to delete
				{
					File.WriteAllLines(savePath, lines.Where((v, j) => j != i)); //Delete setting
				}
			}
		}

		public static void DeleteAllSettings()
		{
			File.Delete(savePath);
		}

		internal static string SettingToString(Setting setting)
		{
			return setting.name + ": " + setting.value;
		}

		public struct Setting
		{
			public string name;
			public string value;

			public Setting(string name, string value)
			{
				this.name = name;
				this.value = value;
			}
		}
	}
}
