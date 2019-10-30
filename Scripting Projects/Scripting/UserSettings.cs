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
			if (savePath != null) SettingsFilePath = savePath;

			using (File.CreateText(SettingsFilePath))
			{
			}
		}

		public static bool IsSetUp()
		{
			if (File.Exists(SettingsFilePath))
				return true;
			return false;
		}

		public static void SaveSetting(string name, object value)
		{
			SaveSetting(new UserSetting(name, value));
		}

		public static void SaveSetting(UserSetting setting)
		{
			if (!IsSetUp()) throw new UserSettingsNotSetUpException();

			if (setting.Name.Contains(':')) throw new NameContainsIllegalCharException();

			var lines = File.ReadAllLines(SettingsFilePath); //All settings in the file
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Split(':')[0].Contains(setting.Name)
				) //This setting should be overwritten as its name matches that of the setting we want to change
					File.WriteAllLines(SettingsFilePath, lines.Where((v, j) => j != i)); //Delete setting
			}

			File.AppendAllText(SettingsFilePath, "\n" + setting); //Write setting
		}

		public static void DeleteSetting(UserSetting setting)
		{
			DeleteSetting(setting.Name);
		}

		public static void DeleteSetting(string name)
		{
			var lines = File.ReadAllLines(SettingsFilePath); //All settings in the file
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Split(':')[0].Contains(name)
				) //This setting should be deleted as it is matches the name of the setting we want to delete
					File.WriteAllLines(SettingsFilePath, lines.Where((v, j) => j != i)); //Delete setting
			}
		}

		public static void DeleteAllSettings()
		{
			File.Delete(SettingsFilePath);
		}


		public static bool ExistsSetting(UserSetting setting)
		{
			return ExistsSetting(setting.Name);
		}

		public static bool ExistsSetting(string name)
		{
			var lines = File.ReadAllLines(SettingsFilePath); //All settings in the file
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Split(':')[0].Contains(name)) //This setting exists
					return true;
			}

			//If we "get here" the setting does not exist
			return false;
		}

		public static UserSetting GetSetting(UserSetting setting)
		{
			return GetSetting(setting.Name);
		}

		public static UserSetting GetSetting(string name)
		{
			if (!IsSetUp()) throw new UserSettingsNotSetUpException();

			if (!ExistsSetting(name)) throw new SettingNotFoundException();

			var lines = File.ReadAllLines(SettingsFilePath); //All settings in the file
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Split(':')[0].Contains(name)) //This setting should be read and returned
					return new UserSetting(line);
			}

			throw new SettingNotFoundException();
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
			var bytes = Convert.FromBase64String(base64String);
			using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
			{
				ms.Write(bytes, 0, bytes.Length);
				ms.Position = 0;
				return new BinaryFormatter().Deserialize(ms);
			}
		}

		public struct UserSetting
		{
			public override string ToString()
			{
				return Name + ":" + ObjectToString(Value);
			}

			public string Name;
			public object Value;

			public UserSetting(string name, object value)
			{
				this.Name = name;
				this.Value = value;
			}

			public UserSetting(string settingString)
			{
				var splitString = settingString.Split(':');
				if (splitString.Length != 2) throw new CorruptUserSettingException();

				Name = splitString[0];
				Value = StringToObject(splitString[1]);
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
	}

	public abstract class UserSettingsException : Exception
	{
	}

	public class CorruptUserSettingException : UserSettingsException
	{
	}

	public class NameContainsIllegalCharException : UserSettingsException
	{
	}

	public class SettingNotFoundException : UserSettingsException
	{
	}

	public class UserSettingsNotSetUpException : UserSettingsException
	{
	}
}