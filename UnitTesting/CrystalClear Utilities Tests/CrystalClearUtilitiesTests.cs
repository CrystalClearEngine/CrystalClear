using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CrystalClear.Utilities;

namespace UnitTests
{
	[TestClass]
	public class CrystalClearUtilitiesTests
	{
		[TestMethod]
		public void SimpleReflectionEqualsTest()
		{
			{
				TestObject testObjectA = new TestObject("Hello!");
				TestObject testObjectB = new TestObject("Hello!");

				Assert.IsTrue(testObjectA.ReflectionEquals(testObjectB), "If this check failed, that means that ReflectionEquals recognized two equal TestObjects as non-equal.");
			}

			{
				TestObject testObjectA = new TestObject("Hello!");
				TestObject testObjectB = new TestObject("This is a different string.");

				Assert.IsFalse(testObjectA.ReflectionEquals(testObjectB), "If this check failed, that means that ReflectionEquals recognized two non-equal TestObjects as equal.");
			}
		}
	}
}
