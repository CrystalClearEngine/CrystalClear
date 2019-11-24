using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrystalClear.HierarchySystem;
using CrystalClear.Standard.HierarchyObjects;

namespace UnitTests
{
	[TestCategory("HierarchySystem")]
	[TestClass]
	public class HierarchyObjectTests
	{
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
	}
}
