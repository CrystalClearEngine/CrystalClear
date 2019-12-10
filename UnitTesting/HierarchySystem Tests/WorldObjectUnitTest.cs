using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrystalClear.Standard.HierarchyObjects;

namespace UnitTests
{
	//[TestCategory("")]
	[TestClass]
	public class WorldObjectTests
	{
		[TestMethod]
		public void WorldObjectInitialization()
		{
			WorldObject worldObject = new WorldObject();
			worldObject.AddChild("child", new WorldObject());
		}
	}
}
