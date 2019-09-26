using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Scripting
{
	public static class UserSettings
	{
		public static string savePath = Environment.CurrentDirectory + @"\settings";

		public static void SetUp(string savePath = null)
		{
			if (savePath != null)
				UserSettings.savePath = savePath;
			using (File.CreateText(UserSettings.savePath)) { }
		}

		public static void SaveSetting(string name, string value) => SaveSetting(new UserSetting(name, value));
		public static void SaveSetting(UserSetting setting)
		{
			if (setting.name.Contains(':'))
			{
				throw NameContainsIllegalCharException;
			}
			string[] lines = File.ReadAllLines(savePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.Split(':')[0].Contains(setting.name)) //This setting should be overwritten as its name matches that of the setting we want to change
				{
					File.WriteAllLines(savePath, lines.Where((v, j) => j != i)); //Delete setting
				}
			}

			File.AppendAllText(savePath, "\n" + setting.ToString()); //Write setting
		}
		public static Exception NameContainsIllegalCharException;

		public static void DeleteSetting(UserSetting setting) => DeleteSetting(setting.name);
		public static void DeleteSetting(string name)
		{
			string[] lines = File.ReadAllLines(savePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.Split(':')[0].Contains(name)) //This setting should be deleted as it is matches the name of the setting we want to delete
				{
					File.WriteAllLines(savePath, lines.Where((v, j) => j != i)); //Delete setting
				}
			}
		}

		public static void DeleteAllSettings()
		{
			File.Delete(savePath);
		}

		public static UserSetting GetSetting(UserSetting setting) => GetSetting(setting.name);
		public static UserSetting GetSetting(string name)
		{
			string[] lines = File.ReadAllLines(savePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.Split(':')[0].Contains(name)) //This setting should be read and returned
				{
					return new UserSetting(line);
				}
			}
			throw SettingNotFound;
		}
		public static Exception SettingNotFound;

		public struct UserSetting
		{
			public override string ToString()
			{
				return name + ":" + value;
			}

			public string name;
			public object value;

			public UserSetting(string name, string value)
			{
				this.name = name;
				this.value = value;
			}

			public UserSetting(string settingString)
			{
				string name = string.Empty, value = null;
				foreach (char c in settingString)
				{
					if (value == null && c != ':')
						name += c;
					else if (name == string.Empty)
						throw CorruptUserSettingException;
					else if (value == null)
					{
						value = string.Empty;
						continue;
					}
					else
						value += c;
				}
				this.name = name;
				this.value = value;
			}
		}
		public static Exception CorruptUserSettingException;

		public static string ObjectToString(object obj)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				new BinaryFormatter().Serialize(ms, obj);
				return Convert.ToBase64String(ms.ToArray());
			}
		}

		public static object StringToObject(string base64String)
		{
			byte[] bytes = Convert.FromBase64String(base64String);
			using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
			{
				ms.Write(bytes, 0, bytes.Length);
				ms.Position = 0;
				return new BinaryFormatter().Deserialize(ms);
			}
		}
	}
}
