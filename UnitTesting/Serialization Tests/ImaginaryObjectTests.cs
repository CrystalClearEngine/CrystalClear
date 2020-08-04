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
			var testObject = new TestObject("Hello!");

			ImaginaryObject imaginaryObject = new ImaginaryConstructableObject(typeof(TestObject),
				new ImaginaryObject[] {new ImaginaryPrimitive("Hello!")});

			var createdTestObject = (TestObject) imaginaryObject.CreateInstance();

			Assert.IsTrue(testObject.Equals(createdTestObject));

			Assert.AreEqual(testObject, createdTestObject);
		}
	}
}