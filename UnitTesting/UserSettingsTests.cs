using CrystalClear.ScriptUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests // TODO implement cleanup
{
	[TestClass]
	public class UserSettingsTests
	{
		[TestInitialize]
		public void Initialize()
		{
			UserSettings.SetUp();
		}

		[TestMethod]
		public void SetUpUserSettings()
		{
			Assert.IsTrue(UserSettings.IsSetUp());
		}

		[TestMethod]
		public void CreateAndReadSetting()
		{
			object objectToStore = "If you can read this, computer, then the save and load has been successful!";
			string settingName = "UnitTestSetting";

			UserSettings.SaveSetting(settingName, objectToStore);

			object resultingObjectAfterLoad = UserSettings.GetSetting(settingName).Value;

			Assert.AreEqual(objectToStore, resultingObjectAfterLoad);
		}

		public void CreateAndReadObjectSetting()
		{
			object objectToStore = new CrystalClear.Standard.HierarchyObjects.WorldObject();
			string settingName = "UnitTestSetting";

			UserSettings.SaveSetting(settingName, objectToStore);

			object resultingObjectAfterLoad = UserSettings.GetSetting(settingName).Value;

			Assert.AreEqual(objectToStore, resultingObjectAfterLoad);
		}

		[TestMethod]
		public void DeleteAllUserSettings()
		{
			UserSettings.DeleteAllSettings();
			Assert.IsFalse(UserSettings.IsSetUp());
		}

		[TestMethod]
		public void DeleteSpecificUserSetting()
		{
			string nameOfObjectToStore = "DeleteSpecificUserSetting UserSetting";
			string objectToStore = "123 ABC";

			UserSettings.SaveSetting(nameOfObjectToStore, objectToStore);
			Assert.IsTrue(UserSettings.ExistsSetting(nameOfObjectToStore));

			UserSettings.DeleteSetting(nameOfObjectToStore);

			Assert.IsFalse(UserSettings.ExistsSetting(nameOfObjectToStore));
		}

		[TestCleanup]
		public void Cleanup()
		{
			UserSettings.DeleteAllSettings();
		}
	}
}