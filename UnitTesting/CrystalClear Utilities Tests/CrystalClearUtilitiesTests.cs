using CrystalClear;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class CrystalClearUtilitiesTests
	{
		[TestMethod]
		public void SimpleReflectionEqualsTest()
		{
			{
				var testObjectA = new TestObject("Hello!");
				var testObjectB = new TestObject("Hello!");

				Assert.IsTrue(testObjectA.ReflectionEquals(testObjectB),
					"If this check failed, that means that ReflectionEquals recognized two equal TestObjects as non-equal.");
			}

			{
				var testObjectA = new TestObject("Hello!");
				var testObjectB = new TestObject("This is a different string.");

				Assert.IsFalse(testObjectA.ReflectionEquals(testObjectB),
					"If this check failed, that means that ReflectionEquals recognized two non-equal TestObjects as equal.");
			}

			{
				var testObjectA = new TestObject("Hey!", true);
				var testObjectB = new TestObject("Hey!", true);

				Assert.IsTrue(testObjectA.ReflectionEquals(testObjectB),
					"If this check failed, that means that ReflectionEquals recognized two equal TestObjects as non-equal.");
				Assert.IsTrue(testObjectA.ReflectionEquals(testObjectB, true),
					"If this check failed, that means that the includePrivate parameter worked incorrectly.");
				Assert.IsTrue(testObjectA.ReflectionEquals(testObjectB, true, true),
					"If this check failed, that means that the includePrivate and/or ignoreProperties parameters worked incorrectly.");
				Assert.IsTrue(testObjectA.ReflectionEquals(testObjectB, ignoreProperties: true),
					"If this check failed, that means that the ignoreProperties parameter worked incorrectly.");
			}
		}
	}
}