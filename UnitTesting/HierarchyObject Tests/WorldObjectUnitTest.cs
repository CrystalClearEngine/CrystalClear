using CrystalClear.ScriptUtilities;
using CrystalClear.Standard.HierarchyObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
			const string Child0Name = "child";
			const string Child1Name = "secondChild";
			const string Child2Name = "nonWorldObjectChild";

			// Initialize the parent WorldObject.
			WorldObject worldObject = new WorldObject();

			worldObject.RunOnLocalHierarchyChange();

			// Add the zeroth child to the parent WorldObject.
			worldObject.AddChild(Child0Name, new WorldObject());

			// Get child 0's Transform.
			Transform child0Transform = (worldObject.LocalHierarchy[Child0Name] as WorldObject).Transform;

			// Make sure that the Transform of child 0 exists in the WorldObject's Transform's Children!
			Assert.IsTrue(worldObject.Transform.Children.Contains(child0Transform));

			// Add the first child to the parent WorldObject.
			worldObject.AddChild(Child1Name, new WorldObject());

			// Get child 1's Transform.
			Transform child1Transform = (worldObject.LocalHierarchy[Child1Name] as WorldObject).Transform;

			// Make sure that the Transform of child 1 exists in the WorldObject's Transform's Children!
			Assert.IsTrue(worldObject.Transform.Children.Contains(child1Transform));

			// Add the second child to the parent WorldObject.
			worldObject.AddChild(Child1Name, new WorldObject());

			// Get child 2's Transform.
			Transform child2Transform = (worldObject.LocalHierarchy[Child1Name] as WorldObject).Transform;

			// Add another child, one that however is a ScriptObject. This cannot be added to the Children's lists, so make sure it works correctly with this scenario.
			worldObject.AddChild(Child2Name, new ScriptObject());

			// Make sure that child 2's transform is added to the children's list (automatically! :D).
			Assert.IsTrue(worldObject.Transform.Children.Contains(child2Transform));
		}
	}
}
