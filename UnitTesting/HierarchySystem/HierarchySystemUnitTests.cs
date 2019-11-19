using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
	[TestClass]
	public class HierarchySystemUnitTest
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
			ho3.ReParent("ho3.3", ho33);
			hierarchyRoot.ReParent("ho1", ho1);
			hierarchyRoot.ReParent("ho2", ho2);
			hierarchyRoot.ReParent("ho3", ho3);
			HierarchySystem.Add("test", hierarchyRoot);
		}

		/// <summary>
		/// Tests the FollowPath system
		/// </summary>
		[TestMethod]
		public void TestFollowPath() // Test for making sure the follow path system works
		{
			HierarchyObject hierarchyObjectToFind = new ScriptObject();
			HierarchyObject hierarchyObjectToFind2 = new UIObject();
			HierarchySystem.LoadedHierarchies["test"].ReParent("followPathTest", hierarchyObjectToFind);
			HierarchySystem.LoadedHierarchies["test"].LocalHierarchy["followPathTest"].ReParent("followPathTest2", hierarchyObjectToFind2);
			Assert.AreEqual(hierarchyObjectToFind, HierarchySystem.FollowPath("test/followPathTest"));
			Assert.AreEqual(hierarchyObjectToFind2, HierarchySystem.FollowPath("test/followPathTest/followPathTest2"));
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
}
