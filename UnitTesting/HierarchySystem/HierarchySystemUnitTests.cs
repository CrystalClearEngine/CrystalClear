using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
	[TestClass]
	public class HierarchySystemUnitTest
	{
		/// <summary>
		/// Initialize needs to run before every test
		/// </summary>
		[TestInitialize]
		public void Initialize()
		{
			HierarchyRoot hierarchyRoot = new HierarchyRoot();
			ScriptObject ho1 = new ScriptObject();
			WorldObject ho2 = new WorldObject();
			UIObject ho33 = new UIObject();
			UIObject ho3 = new UIObject();
			ho3.Add("ho3.3", ho33);
			hierarchyRoot.Add("ho1", ho1);
			hierarchyRoot.Add("ho2", ho2);
			hierarchyRoot.Add("ho3", ho3);
			HierarchySystem.Add("test", hierarchyRoot);
		}

		[TestMethod]
		public void TestFollowPath() // Test for making sure the follow path system works
		{
			HierarchyObject hierarchyObjectToFind = new ScriptObject();
			HierarchySystem.LoadedHierarchies["test"].Add("followPathTest", hierarchyObjectToFind);
			Assert.AreEqual(hierarchyObjectToFind, HierarchySystem.FollowPath("test/followPathTest"));
		}

		/// <summary>
		/// Needs to be run after every test
		/// </summary>
		[TestCleanup]
		public void Cleanup()
		{
			HierarchySystem.LoadedHierarchies = new System.Collections.Generic.Dictionary<string, HierarchyObject>();
		}
	}
}
