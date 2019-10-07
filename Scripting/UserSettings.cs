using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CrystalClear.Scripting
{
	public static class UserSettings
	{
		public static string SettingsFilePath = Environment.CurrentDirectory + @"\settings";

		public static void SetUp(string savePath = null)
		{
			if (savePath != null)
			{
				UserSettings.SettingsFilePath = savePath;
			}

			using (File.CreateText(UserSettings.SettingsFilePath)) { }
		}

		public static bool IsSetUp()
		{
			if (File.Exists(SettingsFilePath))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static void SaveSetting(string name, object value) => SaveSetting(new UserSetting(name, value));
		public static void SaveSetting(UserSetting setting)
		{
			if (!IsSetUp())
			{
				throw new UserSettingsNotSetUpException();
			}

			if (setting.name.Contains(':'))
			{
				throw new NameContainsIllegalCharException();
			}

			string[] lines = File.ReadAllLines(SettingsFilePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.Split(':')[0].Contains(setting.name)) //This setting should be overwritten as its name matches that of the setting we want to change
				{
					File.WriteAllLines(SettingsFilePath, lines.Where((v, j) => j != i)); //Delete setting
				}
			}

			File.AppendAllText(SettingsFilePath, "\n" + setting.ToString()); //Write setting
		}

		public static void DeleteSetting(UserSetting setting) => DeleteSetting(setting.name);
		public static void DeleteSetting(string name)
		{
			string[] lines = File.ReadAllLines(SettingsFilePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.Split(':')[0].Contains(name)) //This setting should be deleted as it is matches the name of the setting we want to delete
				{
					File.WriteAllLines(SettingsFilePath, lines.Where((v, j) => j != i)); //Delete setting
				}
			}
		}

		public static void DeleteAllSettings()
		{
			File.Delete(SettingsFilePath);
		}


		public static bool ExistsSetting(UserSetting setting) => ExistsSetting(setting.name);
		public static bool ExistsSetting(string name)
		{
			string[] lines = File.ReadAllLines(SettingsFilePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.Split(':')[0].Contains(name)) //This setting exists
				{
					return true;
				}
			}
			//If we "get here" the setting does not exist
			return false;
		}

		public static UserSetting GetSetting(UserSetting setting) => GetSetting(setting.name);
		public static UserSetting GetSetting(string name)
		{
			if (!IsSetUp())
			{
				throw new UserSettingsNotSetUpException();
			}

			if (!ExistsSetting(name))
			{
				throw new SettingNotFoundException();
			}

			string[] lines = File.ReadAllLines(SettingsFilePath); //All settings in the file
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.Split(':')[0].Contains(name)) //This setting should be read and returned
				{
					return new UserSetting(line);
				}
			}
			throw new SettingNotFoundException();
		}

		public struct UserSetting
		{
			public override string ToString()
			{
				return name + ":" + ObjectToString(value);
			}

			public string name;
			public object value;

			public UserSetting(string name, object value)
			{
				this.name = name;
				this.value = value;
			}

			public UserSetting(string settingString)
			{
				string[] splitString = settingString.Split(':');
				if (splitString.Length != 2)
				{
					throw new CorruptUserSettingException();
				}

				name = splitString[0];
				value = StringToObject(splitString[1]);
				//string name = string.Empty, stringValue = null;
				//foreach (char c in settingString)
				//{
				//	if (stringValue == null && c != ':')
				//		name += c;
				//	else if (name == string.Empty)
				//		throw new CorruptUserSettingException();
				//	else if (stringValue == null)
				//	{
				//		stringValue = string.Empty;
				//		continue;
				//	}
				//	else
				//		stringValue += c;
				//}
				//this.name = name;
				//value = StringToObject(stringValue);
			}
		}

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

	public abstract class UserSettingsException : Exception { }

	public class CorruptUserSettingException : UserSettingsException { }
	public class NameContainsIllegalCharException : UserSettingsException { }
	public class SettingNotFoundException : UserSettingsException { }
	public class UserSettingsNotSetUpException : UserSettingsException { }
}
