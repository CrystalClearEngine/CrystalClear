using System;
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

		[TestMethod]
		public void VectorOperatorTest() // TODO Move to vector test.
		{
			// Initialization.
			Vector v1 = new Vector(1f);
			Vector v2 = new Vector(1f);
			Vector v3 = new Vector(1f);
			Vector v4 = new Vector(1f);

			// Addition test.
			Assert.IsTrue((v1 + v2 + v3 + v4).Axis[0] == (1f + 1f + 1f + 1f));

			// Subtraction test.
			Assert.IsTrue((v1 - v2 - v3 - v4).Axis[0] == (1f - 1f - 1f - 1f));

			// Multiplication test.
			Assert.IsTrue((v1 * v2 * v3 * v4).Axis[0] == (1f * 1f * 1f * 1f));

			// Subtraction test.
			Assert.IsTrue((v1 / v2 / v3 / v4).Axis[0] == (1f / 1f / 1f / 1f));

			// Equality test.
			Assert.IsTrue(v1 == new Vector(1f));
			Assert.IsTrue(v1 == v2);
			Assert.IsTrue(v2 == v3);
			Assert.IsTrue(v4 == v1);
			Assert.IsTrue(v1.Equals(v1));
			Assert.IsTrue(v1.Equals(v2));
			Assert.IsTrue(v1.Equals(v3));
			Assert.IsTrue(v1.Equals(v4));

			// Inequality test.
			Assert.IsTrue(v1 != new Vector(2f));
			Assert.IsFalse(v1 != v2);
			Assert.IsFalse(v2 != v3);
			Assert.IsFalse(v4 != v1);
		}
	}
}
