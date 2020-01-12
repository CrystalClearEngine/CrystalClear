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
	public class SerializationTests // TODO add cleanup method.
	{
		readonly string path = WorkingPath + @"\StorageTest.bin";

		[TestCleanup]
		public void Cleanup()
		{
			System.IO.File.Delete(path);
		}

		private class ObjectSerializationTestClass : IEquatable<ObjectSerializationTestClass>, IExtraObjectData
		{
			public ObjectSerializationTestClass(string someParameter)
			{
				something = someParameter;
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

			ExtraDataObject IExtraObjectData.GetData()
			{
				return new ExtraDataObject()
				{
					{"SomeData", something},
				};
			}

			void IExtraObjectData.SetData(ExtraDataObject data)
			{
				something = (string)data["SomeData"];
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
		public void ObjectStorageTest()
		{
			var objectToStore = new ObjectSerializationTestClass("Hejs");

			EditorObjectSerialization.StoreToFile(path, typeof(ObjectSerializationTestClass), new[] { "Hejs" }, objectToStore);

			var resultingObjectAfterLoad = (ObjectSerializationTestClass)EditorObjectSerialization.CreateFromStoreFile(path);

			Assert.IsTrue(objectToStore.Equals(resultingObjectAfterLoad));
		}

		[TestMethod]
		public void ObjectSaveTest()
		{
			var objectToStore = new ObjectSerializationTestClass("Hejs");

			EditorObjectSerialization.SaveToFile(path, typeof(ObjectSerializationTestClass), new[] { "Hejs" }, objectToStore);

			var resultingObjectAfterLoad = (ObjectSerializationTestClass)EditorObjectSerialization.CreateFromSaveFile(path);

			Assert.IsTrue(objectToStore.Equals(resultingObjectAfterLoad));
		}

		[TestMethod]
		public void HierarchyObjectStorageTest()
		{
			var objectToStore = new ScriptObject();

			EditorObjectSerialization.StoreToFile(path, typeof(ScriptObject), null, objectToStore);

			var resultingObjectAfterLoad = (ScriptObject)EditorObjectSerialization.CreateFromStoreFile(path);

			Assert.IsTrue(objectToStore.Equals(resultingObjectAfterLoad));
		}

		[TestMethod]
		public void HierarchyObjectSaveTest()
		{
			var objectToStore = new ScriptObject();

			EditorObjectSerialization.SaveToFile(path, typeof(ScriptObject), null, objectToStore);

			var resultingObjectAfterLoad = (ScriptObject)EditorObjectSerialization.CreateFromSaveFile(path);

			Assert.IsTrue(objectToStore.Equals(resultingObjectAfterLoad));
		}
	}
}
