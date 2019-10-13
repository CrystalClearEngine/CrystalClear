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
				object ObjectToStore = "If you can read this, computer, then the save and load has been successful!";
				var NameForSetting = "UnitTestSetting";

				UserSettings.SetUp();
				UserSettings.SaveSetting(NameForSetting, ObjectToStore);

				object ResultingObjectAfterLoad = UserSettings.GetSetting(NameForSetting).value;

				Assert.AreEqual(ObjectToStore, ResultingObjectAfterLoad);

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
				var NameOfObjectToStore = "DeleteSpecificUserSetting UserSetting";
				var ObjectToStore = "123 ABC";

				UserSettings.SetUp();

				UserSettings.SaveSetting(NameOfObjectToStore, ObjectToStore);
				Assert.IsTrue(UserSettings.ExistsSetting(NameOfObjectToStore));

				UserSettings.DeleteSetting(NameOfObjectToStore);

				Assert.IsFalse(UserSettings.ExistsSetting(NameOfObjectToStore));
				UserSettings.DeleteAllSettings();
			}
		}
	}
}