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
		class ObjectSerializationTestClass
		{
			public ObjectSerializationTestClass(string somethign)
			{
				something = somethign;
			}

			string something;

			public override bool Equals(object obj)
			{
				return something == ((ObjectSerializationTestClass)obj).something;
			}
		}

		[TestMethod]
		public void GenericObjectStorageTest()
		{
			string path = WorkingPath + @"\StorageTest.bin";

			var objectToStore = new ObjectSerializationTestClass("Hejs");

			ObjectConstructionStorage<ObjectSerializationTestClass>.StoreToFile(path, new[] { "Hejs" });

			var resultingObjectAfterLoad = ObjectConstructionStorage<ObjectSerializationTestClass>.CreateFromStoreFile(path);

			Assert.IsTrue(objectToStore.Equals(resultingObjectAfterLoad));
		}

		[TestMethod]
		public void GenericObjectSaveTest()
		{
			string path = WorkingPath + @"\StorageTest.bin";

			var objectToStore = new ObjectSerializationTestClass("Hejs");

			ObjectConstructionStorage<ObjectSerializationTestClass>.SaveToFile(path, new[] { "Hejs" });

			var resultingObjectAfterLoad = ObjectConstructionStorage<ObjectSerializationTestClass>.CreateFromSaveFile(path);

			Assert.IsTrue(objectToStore.Equals(resultingObjectAfterLoad));
		}
	}
}
