﻿using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CrystalClear.ScriptUtilities
{
	public static class UserSettings
	{
		// TODO: make parameter that is passed to methods instead of stored here.
		/*volatile (maybe?)*/
		private static string SettingsFilePath = Environment.CurrentDirectory + @"\settings";

		public static void SetUp(string savePath = null)
		{
			if (savePath is not null)
			{
				SettingsFilePath = savePath;
			}

			using (File.CreateText(SettingsFilePath))
			{
			}
		}

		public static bool IsSetUp()
		{
			if (File.Exists(SettingsFilePath))
			{
				return true;
			}

			return false;
		}

		public static void SaveSetting(string name, object value)
		{
			SaveSetting(new UserSetting(name, value));
		}

		public static void SaveSetting(UserSetting setting)
		{
			if (!IsSetUp())
			{
				throw new UserSettingsNotSetUpException();
			}

			if (setting.Name.Contains(':'))
			{
				throw new UserSettingNameContainsColon();
			}

			string[] lines = File.ReadAllLines(SettingsFilePath); // All settings in the file
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Split(':')[0].Contains(setting.Name)
				) // This setting should be overwritten as its name matches that of the setting we want to change
				{
					File.WriteAllLines(SettingsFilePath, lines.Where((v, j) => j != i)); // Delete setting
				}
			}

			File.AppendAllText(SettingsFilePath, "\n" + setting); // Write setting
		}

		public static void DeleteSetting(UserSetting setting)
		{
			DeleteSetting(setting.Name);
		}

		public static void DeleteSetting(string name)
		{
			string[] lines = File.ReadAllLines(SettingsFilePath); // All settings in the file
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Split(':')[0].Contains(name)
				) // This setting should be deleted as it is matches the name of the setting we want to delete
				{
					File.WriteAllLines(SettingsFilePath, lines.Where((v, j) => j != i)); // Delete setting
				}
			}
		}

		public static void DeleteAllSettings()
		{
			File.Delete(SettingsFilePath);
		}


		public static bool ExistsSetting(UserSetting setting) => ExistsSetting(setting.Name);

		public static bool ExistsSetting(string name)
		{
			string[] lines = File.ReadAllLines(SettingsFilePath); // All settings in the file
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Split(':')[0].Contains(name)) // This setting exists
				{
					return true;
				}
			}

			// If we "get here" the setting does not exist
			return false;
		}

		public static UserSetting GetSetting(UserSetting setting) => GetSetting(setting.Name);

		public static UserSetting GetSetting(string name)
		{
			if (!IsSetUp())
			{
				throw new UserSettingsNotSetUpException();
			}

			if (!ExistsSetting(name))
			{
				throw new SettingNotFoundException(name);
			}

			string[] lines = File.ReadAllLines(SettingsFilePath); // All settings in the file
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Split(':')[0].Contains(name)) // This setting should be read and returned
				{
					return new UserSetting(line);
				}
			}

			throw new SettingNotFoundException(name);
		}

		public static string ObjectToJson(object obj)
		{
			return JsonSerializer.Serialize(obj);
		}

		public static T JsonToObject<T>(string json)
		{
			return JsonSerializer.Deserialize<T>(json);
		}

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
		public struct UserSetting : IEquatable<UserSetting>
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
		{
			public string Name;
			public object Value;

			public UserSetting(string name, object value)
			{
				Name = name;
				Value = value;
			}

			public UserSetting(string settingValueString)
			{
				string[] splitString = settingValueString.Split(':');
				if (splitString.Length != 2)
				{
					throw new CorruptUserSettingException(settingValueString);
				}

				Name = splitString[0];
				Value = JsonToObject<object>(splitString[1]);
			}

			public override string ToString() => Name + ":" + ObjectToJson(Value);

			public static bool operator ==(UserSetting left, UserSetting right) => left.Equals(right);

			public static bool operator !=(UserSetting left, UserSetting right) => !(left == right);

			public bool Equals(UserSetting other)
			{
				if (other.Name == Name)
				{
					if (other.Value.Equals(Value))
					{
						return true;
					}
				}

				return false;
			}

			public override bool Equals(object obj) => obj is UserSetting setting && Equals(setting);
		}
	}

	#region Exceptions

	/// <summary>
	///     An exception thrown by a method in UserExceptions
	/// </summary>
	public abstract class UserSettingsException : Exception
	{
	}

	/// <summary>
	///     An exception thrown by a method in UserExceptions that regards a single exception
	/// </summary>
	public abstract class UserSettingException : UserSettingsException
	{
	}

	public class CorruptUserSettingException : UserSettingException
	{
		public CorruptUserSettingException(string UserSettingName)
		{
			this.UserSettingName = UserSettingName;
		}

		public string UserSettingName { get; }
	}

	public class UserSettingNameContainsColon : UserSettingsException
	{
	}

	public class SettingNotFoundException : UserSettingsException
	{
		public string NameOfNotFoundSetting;

		public SettingNotFoundException(string NameOfNotFoundSetting)
		{
			this.NameOfNotFoundSetting = NameOfNotFoundSetting;
		}
	}

	public class UserSettingsNotSetUpException : UserSettingsException
	{
	}

	#endregion
}