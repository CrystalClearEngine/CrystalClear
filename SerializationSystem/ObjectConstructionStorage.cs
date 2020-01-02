using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using K4os.Compression.LZ4.Streams;

namespace CrystalClear.SerializationSystem
{
	/// <summary>
	/// Internal type for use only to data dump.
	/// </summary>
	[Serializable]
	internal struct ObjectStorage
	{
		internal string TypeName;
		internal object[] ConstructorParameters;

		public ObjectStorage(Type objectType, object[] constructorParameters)
		{
			TypeName = objectType.AssemblyQualifiedName;
			ConstructorParameters = constructorParameters;
		}
	}

	public static class ObjectConstructionStorage<T>
    {
		public static void StoreToFile(string path, /*Type objectType,*/ object[] constructorParameters = null)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			using (LZ4EncoderStream compressionStream = LZ4Stream.Encode(fileStream))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				binaryFormatter.Serialize(compressionStream, new ObjectStorage(typeof(T), constructorParameters));
			}
		}

		public static void SaveToFile(string path, object[] constructorParameters = null) // TODO add this
		{

		}

		public static T CreateFromStoreFile(string path)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			using (LZ4DecoderStream decompressionStream = LZ4Stream.Decode(fileStream))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				ObjectStorage deserializedStorage = (ObjectStorage)binaryFormatter.Deserialize(decompressionStream);

				return (T)Activator.CreateInstance(Type.GetType(deserializedStorage.TypeName), deserializedStorage.ConstructorParameters);
			}
		}
	}

	// TODO fix and add this back!
	//public static class ObjectConstructionStorage
	//{
	//	public static void StoreToFile<T>(string path, object[] constructorParameters = null)
	//	{
	//		ObjectConstructionStorage<T>.StoreToFile(path, constructorParameters);
	//	}

	//	public static void SaveToFile<T>(string path, object[] constructorParameters = null)
	//	{
	//		ObjectConstructionStorage<T>.SaveToFile(path, constructorParameters);
	//	}

	//	public static T CreateFromStoreFile<T>(string path)
	//	{
	//		return ObjectConstructionStorage<T>.CreateFromStoreFile(path);
	//	}
	//}
}
