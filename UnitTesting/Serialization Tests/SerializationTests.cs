using System;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.SerializationSystem;
using CrystalClear.Standard.HierarchyObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CrystalClear.CrystalClearInformation;

namespace UnitTests
{
	[TestClass]
	public class SerializationTests
	{
		class MyClass
		{
			public MyClass(string somethign)
			{
				something = somethign;
			}

			string something;

			// override object.Equals
			public override bool Equals(object obj)
			{
				return something == ((MyClass)obj).something;
			}
		}

		[TestMethod]
		public void ObjectStorageTest()
		{
			string path = WorkingPath + @"\StorageTest.bin";

			var objectToStore = new MyClass("Hejs");

			ObjectConstructionStorage<MyClass>.StoreToFile(path, new[] { "Hejs" });

			var resultingObjectAfterLoad = ObjectConstructionStorage<MyClass>.CreateFromStoreFile(path);

			Assert.IsTrue(objectToStore.Equals(resultingObjectAfterLoad));
		}
	}
}
