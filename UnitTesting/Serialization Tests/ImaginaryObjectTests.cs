using CrystalClear.SerializationSystem.ImaginaryObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
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

			TestObject createdTestObject = (TestObject)imaginaryObject.CreateInstance();

			Assert.IsTrue(testObject.Equals(createdTestObject));

			Assert.AreEqual(testObject, createdTestObject);
		}
	}
}