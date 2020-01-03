using System;
using System.Collections.Generic;
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
		private class ObjectSerializationTestClass : IEquatable<ObjectSerializationTestClass>
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

			public bool Equals(ObjectSerializationTestClass other)
			{
				return other != null &&
					   something == other.something;
			}

			public override int GetHashCode()
			{
				return -1851467485 + EqualityComparer<string>.Default.GetHashCode(something);
			}

			public static bool operator ==(ObjectSerializationTestClass left, ObjectSerializationTestClass right)
			{
				return EqualityComparer<ObjectSerializationTestClass>.Default.Equals(left, right);
			}

			public static bool operator !=(ObjectSerializationTestClass left, ObjectSerializationTestClass right)
			{
				return !(left == right);
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

		[TestMethod]
		public void ObjectStorageTest()
		{
			string path = WorkingPath + @"\StorageTest.bin";

			var objectToStore = new ObjectSerializationTestClass("Hejs");

			ObjectConstructionStorage.StoreToFile(path, typeof(ObjectSerializationTestClass), new[] { "Hejs" });

			var resultingObjectAfterLoad = (ObjectSerializationTestClass)ObjectConstructionStorage.CreateFromStoreFile(path);

			Assert.IsTrue(objectToStore.Equals(resultingObjectAfterLoad));
		}

		[TestMethod]
		public void ObjectSaveTest()
		{
			string path = WorkingPath + @"\StorageTest.bin";

			var objectToStore = new ObjectSerializationTestClass("Hejs");

			ObjectConstructionStorage.SaveToFile(path, typeof(ObjectSerializationTestClass), new[] { "Hejs" });

			var resultingObjectAfterLoad = (ObjectSerializationTestClass)ObjectConstructionStorage.CreateFromSaveFile(path);

			Assert.IsTrue(objectToStore.Equals(resultingObjectAfterLoad));
		}
	}
}
