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
			// Instantiation.
			ScriptObject parent = new ScriptObject();
			ScriptObject child = new ScriptObject();

			// Add child.
			string childName = "childScriptObject";
			parent.AddChild(childName, child);

			// Asserts.
			Assert.AreEqual(childName, parent.GetChildName(child));
			Assert.AreEqual(childName, child.Name);
		}

		[TestMethod]
		public void GetRoot()
		{
			// Instantiation.
			ScriptObject root = new ScriptObject();
			ScriptObject child = new ScriptObject();

			// Add child.
			root.AddChild("child", child);

			// Assert.
			Assert.IsTrue(ReferenceEquals(root, child.Root));
		}

		[TestMethod]
		public void GetParent() // TODO: Make generic versions of this and similar test classes testing all HierarchyObject types. Maybe run these tests when custom HierarchyObject types are created to make sure they are compatible.
		{
			// Instantiation.
			ScriptObject parent = new ScriptObject();
			ScriptObject child = new ScriptObject();

			// Make sure that the child does not have a parent by default.
			Assert.IsNull(child.Parent);

			// Add child.
			parent.AddChild("child", child);

			// Assert.
			Assert.IsTrue(ReferenceEquals(parent, child.Parent));
		}

		[TestMethod]
		public void GetIsRoot()
		{
			// Instantiation.
			ScriptObject root = new ScriptObject();
			ScriptObject child = new ScriptObject();

			// Add child.
			root.AddChild("child", child);

			// Asserts.
			Assert.IsTrue(root.IsRoot);
			Assert.IsFalse(child.IsRoot);
		}
	}
}
