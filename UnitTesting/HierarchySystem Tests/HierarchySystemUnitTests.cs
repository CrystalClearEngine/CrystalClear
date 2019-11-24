using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestCategory("HierarchySystem")]
	[TestClass]
	public class HierarchySystemUnitTests
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
			HierarchySystem.AddHierarchy("test", hierarchyRoot);
		}

		/// <summary>
		/// Tests the FollowPath system.
		/// </summary>
		[TestMethod]
		public void TestFollowPath()
		{
			HierarchyObject hierarchyObjectToFind = new ScriptObject();
			HierarchyObject hierarchyObjectToFind2 = new UIObject();
			HierarchySystem.LoadedHierarchies["test"].AddChild("followPathTest", hierarchyObjectToFind);
			HierarchySystem.LoadedHierarchies["test"].LocalHierarchy["followPathTest"].AddChild("followPathTest2", hierarchyObjectToFind2);
			Assert.AreEqual(hierarchyObjectToFind, HierarchySystem.FollowPath("test/followPathTest"));
			Assert.AreEqual(hierarchyObjectToFind2, HierarchySystem.FollowPath("test/followPathTest/followPathTest2"));
		}

		/// <summary>
		/// Tests the SetHierarchyName() method in HierarchySystem.
		/// </summary>
		[TestMethod]
		public void TestSetHierarchyName()
		{
			string newName = "newName";
			HierarchyObject hierarchyObject = HierarchySystem.LoadedHierarchies["test"];
			HierarchySystem.SetHierarchyName("test", newName);
			Assert.IsTrue(HierarchySystem.LoadedHierarchies.ContainsKey(newName));
			Assert.AreEqual(HierarchySystem.GetHierarchyName(hierarchyObject), newName);
		}

		/// <summary>
		/// Tests the GetHierarchyName() method in HierarchySystem.
		/// </summary>
		[TestMethod]
		public void TestGetHierarchyName()
		{
			HierarchyRoot hierarchyRoot = new HierarchyRoot();
			string hierarchyName = "GetThisName";
			HierarchySystem.AddHierarchy(hierarchyName, hierarchyRoot);
			Assert.AreEqual(HierarchySystem.GetHierarchyName(hierarchyRoot), hierarchyName);
		}

		/// <summary>
		/// Cleanup to be run after every test.
		/// </summary>
		[TestCleanup]
		public void Cleanup()
		{
			HierarchySystem.LoadedHierarchies = new System.Collections.Generic.Dictionary<string, HierarchyObject>();
		}
	}

	[TestCategory("HierarchySystem")]
	[TestClass]
	public class HierarchyObjectUnitTests
	{

	}
}
