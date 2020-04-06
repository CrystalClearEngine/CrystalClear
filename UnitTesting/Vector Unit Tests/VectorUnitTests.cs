using System;
using CrystalClear.ScriptUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class VectorUnitTests
	{
		[TestMethod]
		public void VectorOperatorTest()
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
