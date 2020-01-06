using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using K4os.Compression.LZ4.Streams;

namespace CrystalClear.SerializationSystem
{
	public static class ObjectConstructionStorage
	{
		#region Creators
		public static object CreateFromSaveFile(string path)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ObjectStorage));

				ObjectStorage deserializedStorage = (ObjectStorage)dataContractSerializer.ReadObject(fileStream);

				object obj = Activator.CreateInstance(Type.GetType(deserializedStorage.typeName), deserializedStorage.constructorParameters);

				// Set the data.
				(obj as IExtraObjectData)?.SetData(deserializedStorage.extraData);

				return obj;
			}
		}

		public static object CreateFromStoreFile(string path)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			using (LZ4DecoderStream decompressionStream = LZ4Stream.Decode(fileStream))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				ObjectStorage deserializedStorage = (ObjectStorage)binaryFormatter.Deserialize(decompressionStream);

				object obj = Activator.CreateInstance(Type.GetType(deserializedStorage.typeName), deserializedStorage.constructorParameters);

				// Set the data.
				(obj as IExtraObjectData)?.SetData(deserializedStorage.extraData);

				return obj;
			}
		}
		#endregion

		#region SaveAndStores
		public static void SaveToFile(string path, Type type, object[] constructorParameters = null, object obj = null)
		{
			// Gets the extra data interface if it is present on the type.
			IExtraObjectData dataInterface = obj as IExtraObjectData;

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ObjectStorage));

				dataContractSerializer.WriteObject(fileStream, new ObjectStorage(type, constructorParameters, dataInterface));
			}
		}

		public static void StoreToFile(string path, Type type, object[] constructorParameters = null, object obj = null)
		{
			// Gets the extra data interface if it is present on the type.
			IExtraObjectData dataInterface = obj as IExtraObjectData;

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			using (LZ4EncoderStream compressionStream = LZ4Stream.Encode(fileStream))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				binaryFormatter.Serialize(compressionStream, new ObjectStorage(type, constructorParameters, dataInterface)); 
			}
		}
		#endregion
	}
}
