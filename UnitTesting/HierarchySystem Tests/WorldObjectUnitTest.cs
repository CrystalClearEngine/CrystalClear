using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrystalClear.Standard.HierarchyObjects;
using System.Linq;

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
			const string Name = "child";
			const string Name1 = "secondChild";
			const string Name2 = "nonWorldObjectChild";

			WorldObject worldObject = new WorldObject();
			worldObject.AddChild(Name, new WorldObject());
			Transform child1Transform = (worldObject.LocalHierarchy[Name] as WorldObject).Transform;
			Assert.IsTrue(worldObject.Transform.Children.Contains(child1Transform));
			worldObject.AddChild(Name1, new WorldObject());
			Transform child2Transform = (worldObject.LocalHierarchy[Name1] as WorldObject).Transform;
			worldObject.AddChild(Name2, new ScriptObject());
			Assert.IsTrue(worldObject.Transform.Children.Contains(child2Transform));
		}
	}
}
