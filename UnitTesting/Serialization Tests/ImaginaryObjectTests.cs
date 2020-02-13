using System;
using System.Collections.Generic;
using CrystalClear.SerializationSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	class TestObject : IEquatable<TestObject>
	{
		public TestObject(string stringData)
		{
			StringData = stringData;
		}

		public string StringData;

		public override bool Equals(object obj)
		{
			return Equals(obj as TestObject);
		}

		public bool Equals(TestObject other)
		{
			return other != null &&
				   StringData == other.StringData;
		}

		public override int GetHashCode()
		{
			return 1045584480 + EqualityComparer<string>.Default.GetHashCode(StringData);
		}

		public static bool operator ==(TestObject left, TestObject right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TestObject left, TestObject right)
		{
			return !(left == right);
		}
	}

	[TestCategory("ImaginaryObjects")]
	[TestClass]
	public class ImaginaryObjectTests
	{
		[TestMethod]
		public void TestImaginaryObject()
		{
			TestObject testObject = new TestObject("Hello!");

			ImaginaryObject imaginaryObject = new ImaginaryObject(typeof(TestObject),
														 new ImaginaryObject[] { new ImaginaryPrimitive("Hello!") });

			Assert.IsTrue(testObject == (TestObject)imaginaryObject.CreateInstance());

			Assert.AreEqual(testObject, (TestObject)imaginaryObject.CreateInstance());
		}
	}
}