using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestCategory("HierarchySystem")]
	[TestClass]
	public class HierarchySystemTests
	{
		/// <summary>
		/// Initialize needs to run before every test.
		/// </summary>
		[TestInitialize]
		public void Initialize()
		{
			HierarchyRoot hierarchyRoot = new HierarchyRoot();
			ScriptObject ho1 = new ScriptObject();
			WorldObject ho2 = new WorldObject();
			UIObject ho33 = new UIObject();
			UIObject ho3 = new UIObject();
			ho3.AddChild("ho3.3", ho33);
			hierarchyRoot.AddChild("ho1", ho1);
			hierarchyRoot.AddChild("ho2", ho2);
			hierarchyRoot.AddChild("ho3", ho3);
			HierarchyManager.AddHierarchy("test", hierarchyRoot);
		}

		/// <summary>
		/// Tests the FollowPath system.
		/// </summary>
		[TestMethod]
		public void TestFollowPath()
		{
			HierarchyObject hierarchyObjectToFind = new ScriptObject();
			HierarchyObject hierarchyObjectToFind2 = new UIObject();
			HierarchyManager.LoadedHierarchies["test"].AddChild("followPathTest", hierarchyObjectToFind);
			HierarchyManager.LoadedHierarchies["test"].LocalHierarchy["followPathTest"].AddChild("followPathTest2", hierarchyObjectToFind2);
			Assert.AreEqual(hierarchyObjectToFind, HierarchyManager.FollowPath("test/followPathTest"));
			Assert.AreEqual(hierarchyObjectToFind2, HierarchyManager.FollowPath("test/followPathTest/followPathTest2"));
		}

		/// <summary>
		/// Tests the SetHierarchyName() method in HierarchyManager.
		/// </summary>
		[TestMethod]
		public void SetHierarchyName()
		{
			string newName = "newName";
			HierarchyObject hierarchyObject = HierarchyManager.LoadedHierarchies["test"];
			HierarchyManager.SetHierarchyName("test", newName);
			Assert.IsTrue(HierarchyManager.LoadedHierarchies.ContainsKey(newName));
			Assert.AreEqual(HierarchyManager.GetHierarchyName(hierarchyObject), newName);
		}

		/// <summary>
		/// Tests the GetHierarchyName() method in HierarchyManager.
		/// </summary>
		[TestMethod]
		public void GetHierarchyName()
		{
			HierarchyRoot hierarchyRoot = new HierarchyRoot();
			string hierarchyName = "GetThisName";
			HierarchyManager.AddHierarchy(hierarchyName, hierarchyRoot);
			Assert.AreEqual(HierarchyManager.GetHierarchyName(hierarchyRoot), hierarchyName);
		}

		/// <summary>
		/// Cleanup to be run after every test.
		/// </summary>
		[TestCleanup]
		public void Cleanup()
		{
			HierarchyManager.LoadedHierarchies = new System.Collections.Generic.Dictionary<string, HierarchyObject>();
		}
	}
}
