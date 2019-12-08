using CrystalClear.Standard.HierarchyObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestCategory("HierarchySystem")]
	[TestClass]
	public class HierarchyObjectTests
	{
		/// <summary>
		/// Test for ensuring that the HierarchyObject's GetChildName() method is functional and works as expected.
		/// </summary>
		[TestMethod]
		public void GetChildName()
		{
			ScriptObject scriptObject = new ScriptObject();
			ScriptObject childScriptObject = new ScriptObject();
			string childName = "childScriptObject";
			scriptObject.AddChild(childName, childScriptObject);
			Assert.AreEqual(childName, scriptObject.GetChildName(childScriptObject));
			Assert.AreEqual(childName, childScriptObject.Name);
		}

		[TestMethod]
		public void GetRoot()
		{
			ScriptObject root = new ScriptObject();
			ScriptObject child = new ScriptObject();
			root.AddChild("child", child);

			Assert.IsTrue(ReferenceEquals(root, child.Root));
		}
	}
}
