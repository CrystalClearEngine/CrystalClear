using CrystalClear.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	public class UserSettingsTests
	{
		[TestClass]
		public class SetUpUserSettingsTest
		{
			[TestMethod]
			public void SetUpUserSettings()
			{
				UserSettings.SetUp();
				Assert.IsTrue(UserSettings.IsSetUp());
				UserSettings.DeleteAllSettings();
			}
		}

		[TestClass]
		public class CreateAndReadSettingTest
		{
			[TestMethod]
			public void CreateAndReadSetting()
			{
				object objectToStore = "If you can read this, computer, then the save and load has been successful!";
				var nameForSetting = "UnitTestSetting";

				UserSettings.SetUp();
				UserSettings.SaveSetting(nameForSetting, objectToStore);

				object resultingObjectAfterLoad = UserSettings.GetSetting(nameForSetting).Value;

				Assert.AreEqual(objectToStore, resultingObjectAfterLoad);

				UserSettings.DeleteAllSettings();
			}
		}

		[TestClass]
		public class DeleteUserSettingsTest
		{
			[TestMethod]
			public void DeleteAllUserSettings()
			{
				UserSettings.SetUp();
				UserSettings.DeleteAllSettings();
				Assert.IsFalse(UserSettings.IsSetUp());
			}

			[TestMethod]
			public void DeleteSpecificUserSetting()
			{
				var nameOfObjectToStore = "DeleteSpecificUserSetting UserSetting";
				var objectToStore = "123 ABC";

				UserSettings.SetUp();

				UserSettings.SaveSetting(nameOfObjectToStore, objectToStore);
				Assert.IsTrue(UserSettings.ExistsSetting(nameOfObjectToStore));

				UserSettings.DeleteSetting(nameOfObjectToStore);

				Assert.IsFalse(UserSettings.ExistsSetting(nameOfObjectToStore));
				UserSettings.DeleteAllSettings();
			}
		}
	}
}