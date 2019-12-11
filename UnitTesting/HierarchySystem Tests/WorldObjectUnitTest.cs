using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrystalClear.Standard.HierarchyObjects;
using System.Linq;
using CrystalClear.ScriptUtilities;

namespace UnitTests
{
	[TestCategory("HierarchySystem")]
	[TestClass]
	public class WorldObjectTests
	{
		[TestMethod]
		public void WorldObjectInitialization()
		{
			new WorldObject();
		}

		[TestMethod]
		public void WorldObjectChildren()
		{
			WorldObject worldObject = new WorldObject();
			worldObject.AddChild("child", new WorldObject());
		}

		[TestMethod]
		public void WorldObjectChildrenTransformUpdating()
		{
			// Constants.
			const string Child = "child";
			const string Child1 = "secondChild";
			const string Child2 = "nonWorldObjectChild";

			// Initialize the parent WorldObject.
			WorldObject worldObject = new WorldObject();

			// Add the first child to the parent WorldObject.
			worldObject.AddChild(Child, new WorldObject());

			// Get child 1's Transform.
			Transform child1Transform = (worldObject.LocalHierarchy[Child] as WorldObject).Transform;

			// Make sure that the Transform of child 1 exists in the WorldObject's Transform's Children!
			Assert.IsTrue(worldObject.Transform.Children.Contains(child1Transform));

			// Add the second child to the parent WorldObject.
			worldObject.AddChild(Child1, new WorldObject());

			// Get child 2's Transform.
			Transform child2Transform = (worldObject.LocalHierarchy[Child1] as WorldObject).Transform;

			// Add another child, one that however is a ScriptObject. This cannot be added to the Children's lists, so make sure it works correctly with this scenario.
			worldObject.AddChild(Child2, new ScriptObject());

			// Make sure that child 2's transform is added to the children's list (automatically! :D).
			Assert.IsTrue(worldObject.Transform.Children.Contains(child2Transform));
		}
	}
}
