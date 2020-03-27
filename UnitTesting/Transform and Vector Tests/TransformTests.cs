using CrystalClear.ScriptUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class TransformTest
	{
		[TestMethod]
		public void LocalPositionAndGlobalPositionUpdateTest()
		{
			// Initialization.
			Transform baseTransform = new Transform(3);
			Transform childTransform = new Transform(3);

			// Add childTransform to baseTransform.
			baseTransform.AddChildTransform(childTransform);

			// Increase the global position of the baseTransform by five on the second axis (y).
			baseTransform.GlobalPosition += new Vector(0f, 5f, 0f);

			// Asserts.
			Assert.IsTrue(childTransform.LocalPosition == new Vector(0f, 0f, 0f));
			Assert.IsTrue(childTransform.GlobalPosition == new Vector(0f, 5f, 0f));
		}
	}
}
