using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;
using System.Diagnostics;

namespace UnitTesting
{
	[TestClass]
	public class HierarchySystemUnitTest
	{
		[TestInitialize]
		public void SetUpHierarchySystem()
		{
			HierarchyRoot hierarchyRoot = new HierarchyRoot();
			ScriptObject ho1 = new ScriptObject();
			WorldObject ho2 = new WorldObject();
			UIObject ho33 = new UIObject();
			UIObject ho3 = new UIObject(); ho3.LocalHierarchy.Add("ho3.3", ho33);
			hierarchyRoot.HierarchyObjects.Add("ho1", ho1);
			hierarchyRoot.HierarchyObjects.Add("ho2", ho2);
			hierarchyRoot.HierarchyObjects.Add("ho3", ho3);
			HierarchySystem.LoadedHierarchies.Add("test", hierarchyRoot);
		}
		
		[TestMethod]
		public void TestFollowPath()
		{
			HierarchySystem.FollowPath("test/ho1");
		}

		/// <summary>
		/// Needs to be called in between each test for cleaning up
		/// </summary>
		[TestCleanup]
		public void Cleanup()
		{
			HierarchySystem.LoadedHierarchies = new System.Collections.Generic.Dictionary<string, HierarchyRoot>();
		}
	}
}
